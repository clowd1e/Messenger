using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Messages.Errors;
using Messenger.Domain.Aggregates.Messages.ValueObjects;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.UpdateMessage
{
    internal sealed class UpdateMessageCommandHandler
        : ICommandHandler<UpdateMessageCommand, UpdateMessageResponse>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMessageCommandHandler(
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

        public async Task<Result<UpdateMessageResponse>> Handle(
            UpdateMessageCommand command,
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
                return Result.Failure<UpdateMessageResponse>(ChatErrors.NotFound);
            }

            // Check if user is in chat
            var isUserInChat = chat.Participants.Any(p => p.Id == userId);

            if (!isUserInChat)
            {
                return Result.Failure<UpdateMessageResponse>(ChatErrors.UserNotInChat);
            }

            // Retrieve message
            var message = await _messageRepository.GetByIdAsync(
                new MessageId(command.MessageId),
                cancellationToken);

            if (message is null)
            {
                return Result.Failure<UpdateMessageResponse>(MessageErrors.NotFound);
            }

            // Check if user is the sender of the message
            if (message.User?.Id != userId)
            {
                return Result.Failure<UpdateMessageResponse>(MessageErrors.UnauthorizedToUpdate);
            }

            // Create new message content
            var contentResult = MessageContent.Create(command.NewContent);

            if (contentResult.IsFailure)
            {
                return Result.Failure<UpdateMessageResponse>(contentResult.Error);
            }

            // Update message
            var updatedAt = Timestamp.UtcNow();
            var updateResult = message.Update(contentResult.Value, updatedAt);

            if (updateResult.IsFailure)
            {
                return Result.Failure<UpdateMessageResponse>(updateResult.Error);
            }

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var participantsIds = chat.Participants.Select(p => p.Id.Value).ToList();

            return new UpdateMessageResponse(
                participantsIds,
                command.ChatId,
                command.MessageId,
                command.NewContent,
                updatedAt.Value);
        }
    }
}
