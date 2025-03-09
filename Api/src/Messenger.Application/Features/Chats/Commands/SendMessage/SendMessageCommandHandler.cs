using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.SendMessage
{
    internal sealed class SendMessageCommandHandler
        : IRequestHandler<SendMessageCommand, Result<MessageResponse>>
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

        public async Task<Result<MessageResponse>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                throw new AuthenticatedUserNotFoundException();
            }

            var chatId = new ChatId(request.ChatId);

            var chat = await _chatRepository.GetByIdWithUsersAsync(
                chatId,
                cancellationToken);

            if (chat is null)
            {
                return Result.Failure<MessageResponse>(ChatErrors.NotFound);
            }

            var isUserInChat = await _chatRepository.IsUserInChatAsync(
                userId,
                chatId,
                cancellationToken);

            if (!isUserInChat)
            {
                return Result.Failure<MessageResponse>(ChatErrors.UserNotInChat);
            }

            var messageRequestModel = new CreateMessageRequestModel(
                request.Message);

            var mappingResult = _commandMapper.Map(messageRequestModel);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<MessageResponse>(mappingResult.Error);
            }

            var message = mappingResult.Value;

            message.SetUser(user);
            message.SetChat(chat);

            var addMessageToChatResult = chat.AddMessage(message);

            if (addMessageToChatResult.IsFailure)
            {
                return Result.Failure<MessageResponse>(addMessageToChatResult.Error);
            }

            var addMessageToUserResult = user.AddMessage(message);

            if (addMessageToUserResult.IsFailure)
            {
                return Result.Failure<MessageResponse>(addMessageToUserResult.Error);
            }

            await _messageRepository.InsertAsync(message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var messageResponse = _messageMapper.Map(message);

            return Result.Success(messageResponse);
        }
    }
}
