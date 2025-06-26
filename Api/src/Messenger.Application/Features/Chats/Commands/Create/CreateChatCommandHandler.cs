using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.Create
{
    internal sealed class CreateChatCommandHandler
        : ICommandHandler<CreateChatCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<CreateChatRequestModel, Result<Chat>> _chatMapper;
        private readonly Mapper<CreateMessageRequestModel, Result<Message>> _messageMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateChatCommandHandler(
            IUserRepository userRepository,
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IUserContextService<Guid> userContextService,
            Mapper<CreateChatRequestModel, Result<Chat>> chatMapper,
            Mapper<CreateMessageRequestModel, Result<Message>> messageMapper,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _userContextService = userContextService;
            _chatMapper = chatMapper;
            _messageMapper = messageMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateChatCommand request,
            CancellationToken cancellationToken)
        {
            var inviterId = new UserId(_userContextService.GetAuthenticatedUserId());

            var inviter = await _userRepository.GetByIdWithChatsAsync(
                inviterId, cancellationToken);

            if (inviter is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            var inviteeId = new UserId(request.InviteeId);

            var invitee = await _userRepository.GetByIdAsync(inviteeId, cancellationToken);

            if (invitee is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            if (inviter.Id == invitee.Id)
            {
                return Result.Failure<Guid>(ChatErrors.ChatWithSameUser);
            }

            var chatExists = await _chatRepository.ExistsAsync(inviterId, inviteeId, cancellationToken);

            if (chatExists)
            {
                return Result.Failure<Guid>(ChatErrors.ChatAlreadyExists);
            }

            var requestModel = new CreateChatRequestModel(inviter, invitee);

            var mappingResult = _chatMapper.Map(requestModel);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var chat = mappingResult.Value;

            var messageRequestModel = new CreateMessageRequestModel(
                request.Message);

            var messageMappingResult = _messageMapper.Map(messageRequestModel);

            if (messageMappingResult.IsFailure)
            {
                return Result.Failure<Guid>(messageMappingResult.Error);
            }

            var message = messageMappingResult.Value;

            message.SetUser(inviter);
            message.SetChat(chat);

            var addMessageToChatResult = chat.AddMessage(message);

            if (addMessageToChatResult.IsFailure)
            {
                return Result.Failure<Guid>(addMessageToChatResult.Error);
            }

            var addMessageToUserResult = inviter.AddMessage(message);

            if (addMessageToUserResult.IsFailure)
            {
                return Result.Failure<Guid>(addMessageToUserResult.Error);
            }

            await _chatRepository.InsertAsync(chat, cancellationToken);
            await _messageRepository.InsertAsync(message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(chat.Id.Value);
        }
    }
}
