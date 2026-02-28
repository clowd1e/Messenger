using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Chats.DTO.RequestModels;
using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.SendMessage
{
    internal sealed class SendMessageCommandHandler
        : ICommandHandler<SendMessageCommand, SendMessageResponse>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<CreateMessageRequestModel, Result<Message>> _commandMapper;
        private readonly Mapper<Message, MessageResponse> _messageMapper;
        private readonly IUnitOfWork _unitOfWork;

        public SendMessageCommandHandler(
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IUserRepository userRepository,
            IUserContextService<Guid> userContextService,
            Mapper<CreateMessageRequestModel, Result<Message>> commandMapper,
            Mapper<Message, MessageResponse> messageMapper,
            IUnitOfWork unitOfWork)
        {
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _userContextService = userContextService;
            _commandMapper = commandMapper;
            _messageMapper = messageMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SendMessageResponse>> Handle(
            SendMessageCommand command,
            CancellationToken cancellationToken)
        {
            // Retrieve user
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                throw new AuthenticatedUserNotFoundException();
            }

            // Retrieve chat
            var chatId = new ChatId(command.ChatId);

            var chat = await _chatRepository.GetByIdWithUsersAndLastMessageAsync(
                userId,
                chatId,
                cancellationToken);

            if (chat is null)
            {
                return Result.Failure<SendMessageResponse>(ChatErrors.NotFound);
            }

            // Check if user is in chat
            var isUserInChat = chat.Participants.Any(p => p.Id == userId);

            if (!isUserInChat)
            {
                return Result.Failure<SendMessageResponse>(ChatErrors.UserNotInChat);
            }

            // Map command to message
            var messageRequestModel = new CreateMessageRequestModel(
                command.Message);

            var mappingResult = _commandMapper.Map(messageRequestModel);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<SendMessageResponse>(mappingResult.Error);
            }

            var message = mappingResult.Value;

            message.SetUser(user);
            message.SetChat(chat);

            // Add message to user
            var addMessageToUserResult = user.AddMessage(message);

            if (addMessageToUserResult.IsFailure)
            {
                return Result.Failure<SendMessageResponse>(addMessageToUserResult.Error);
            }

            // Add message to chat
            var addMessageToChatResult = chat.AddMessage(message);

            if (addMessageToChatResult.IsFailure)
            {
                return Result.Failure<SendMessageResponse>(addMessageToChatResult.Error);
            }

            await _messageRepository.InsertAsync(message, cancellationToken);

            var messageResponse = _messageMapper.Map(message);

            // Save changes to database
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var participantsIds = chat.Participants.Select(p => p.Id.Value).ToList();

            var response = new SendMessageResponse(participantsIds, chat.Id.Value, messageResponse);

            return response;
        }
    }
}
