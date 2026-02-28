using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Messages.Errors;
using Messenger.Domain.Aggregates.Messages.ValueObjects;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.DeleteMessageForUser
{
    internal sealed class DeleteMessageForUserCommandHandler
        : ICommandHandler<DeleteMessageForUserCommand, MessageDeletedForUserResponse>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMessageForUserCommandHandler(
            IMessageRepository messageRepository,
            IChatRepository chatRepository,
            IUserRepository userRepository,
            IUserContextService<Guid> userContextService,
            IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _userContextService = userContextService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MessageDeletedForUserResponse>> Handle(
            DeleteMessageForUserCommand command,
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
            var chat = await _chatRepository.GetByIdWithUsersAsync(
                new ChatId(command.ChatId),
                cancellationToken);

            if (chat is null)
            {
                return Result.Failure<MessageDeletedForUserResponse>(ChatErrors.NotFound);
            }

            // Check if user is in chat
            var isUserInChat = chat.Participants.Any(p => p.Id == userId);

            if (!isUserInChat)
            {
                return Result.Failure<MessageDeletedForUserResponse>(ChatErrors.UserNotInChat);
            }

            // Retrieve message
            var message = await _messageRepository.GetByIdAsync(
                new MessageId(command.MessageId),
                cancellationToken);

            if (message is null)
            {
                return Result.Failure<MessageDeletedForUserResponse>(MessageErrors.NotFound);
            }

            // Delete message for user
            var deleteResult = message.DeleteForUser(user);

            if (deleteResult.IsFailure)
            {
                return Result.Failure<MessageDeletedForUserResponse>(deleteResult.Error);
            }

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new MessageDeletedForUserResponse(
                command.ChatId,
                command.MessageId,
                userId.Value);
        }
    }
}
