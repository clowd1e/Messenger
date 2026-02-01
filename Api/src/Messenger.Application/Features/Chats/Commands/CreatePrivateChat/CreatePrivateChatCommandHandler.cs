using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.RequestModels;
using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.CreatePrivateChat
{
    internal sealed class CreatePrivateChatCommandHandler
        : ICommandHandler<CreatePrivateChatCommand, PrivateChatResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<CreatePrivateChatRequestModel, Result<PrivateChat>> _privateChatMapper;
        private readonly Mapper<Chat, ChatResponse> _chatResponseMapper;
        private readonly Mapper<CreateMessageRequestModel, Result<Message>> _messageMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePrivateChatCommandHandler(
            IUserRepository userRepository,
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IUserContextService<Guid> userContextService,
            Mapper<CreatePrivateChatRequestModel, Result<PrivateChat>> privateChatMapper,
            Mapper<Chat, ChatResponse> chatResponseMapper,
            Mapper<CreateMessageRequestModel, Result<Message>> messageMapper,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _userContextService = userContextService;
            _privateChatMapper = privateChatMapper;
            _chatResponseMapper = chatResponseMapper;
            _messageMapper = messageMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PrivateChatResponse>> Handle(
            CreatePrivateChatCommand command,
            CancellationToken cancellationToken)
        {
            // Retrieve inviter
            var inviterId = new UserId(_userContextService.GetAuthenticatedUserId());

            var inviter = await _userRepository.GetByIdWithPrivateChatsAsync(
                inviterId, cancellationToken);

            if (inviter is null)
            {
                return Result.Failure<PrivateChatResponse>(UserErrors.NotFound);
            }

            // Retrieve invitee
            var inviteeId = new UserId(command.InviteeId);

            var invitee = await _userRepository.GetByIdAsync(inviteeId, cancellationToken);

            if (invitee is null)
            {
                return Result.Failure<PrivateChatResponse>(UserErrors.NotFound);
            }

            // Check if the invitee is inviter
            if (inviter.Id == invitee.Id)
            {
                return Result.Failure<PrivateChatResponse>(ChatErrors.ChatWithSameUser);
            }

            // Check if the chat already exists
            var chatExists = await _chatRepository.PrivateChatExistsAsync(inviterId, inviteeId, cancellationToken);

            if (chatExists)
            {
                return Result.Failure<PrivateChatResponse>(ChatErrors.ChatAlreadyExists);
            }

            // Map to PrivateChat
            var requestModel = new CreatePrivateChatRequestModel(inviter, invitee);

            var mappingResult = _privateChatMapper.Map(requestModel);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<PrivateChatResponse>(mappingResult.Error);
            }

            var privateChat = mappingResult.Value;

            // Map first message
            var messageRequestModel = new CreateMessageRequestModel(command.Message);

            var messageMappingResult = _messageMapper.Map(messageRequestModel);

            if (messageMappingResult.IsFailure)
            {
                return Result.Failure<PrivateChatResponse>(messageMappingResult.Error);
            }

            var message = messageMappingResult.Value;

            message.SetUser(inviter);
            message.SetChat(privateChat);

            // Add message to chat and user
            var addMessageToChatResult = privateChat.AddMessage(message);

            if (addMessageToChatResult.IsFailure)
            {
                return Result.Failure<PrivateChatResponse>(addMessageToChatResult.Error);
            }

            var addMessageToUserResult = inviter.AddMessage(message);

            if (addMessageToUserResult.IsFailure)
            {
                return Result.Failure<PrivateChatResponse>(addMessageToUserResult.Error);
            }

            // Insert chat and message
            await _chatRepository.InsertPrivateChatAsync(privateChat, cancellationToken);
            await _messageRepository.InsertAsync(message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = (_chatResponseMapper.Map(privateChat) as PrivateChatResponse)!;

            return Result.Success(result);
        }
    }
}
