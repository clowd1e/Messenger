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

namespace Messenger.Application.Features.Chats.Commands.DeleteMessageForEveryone
{
    internal sealed class DeleteMessageForEveryoneCommandHandler
        : ICommandHandler<DeleteMessageForEveryoneCommand, MessageDeletedForEveryoneResponse>
    {
        private readonly IMessageRepository _messageRespository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMessageForEveryoneCommandHandler(
            IMessageRepository messageRespository,
            IChatRepository chatRepository,
            IUserRepository userRepository,
            IUserContextService<Guid> userContextService,
            IUnitOfWork unitOfWork)
        {
            _messageRespository = messageRespository;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _userContextService = userContextService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MessageDeletedForEveryoneResponse>> Handle(
            DeleteMessageForEveryoneCommand command,
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
                return Result.Failure<MessageDeletedForEveryoneResponse>(ChatErrors.NotFound);
            }

            // Check if user is in chat
            var isUserInChat = chat.Participants.Any(p => p.Id == userId);

            if (!isUserInChat)
            {
                return Result.Failure<MessageDeletedForEveryoneResponse>(ChatErrors.UserNotInChat);
            }

            // Retrieve message
            var message = await _messageRespository.GetByIdAsync(
                new MessageId(command.MessageId),
                cancellationToken);

            if (message is null)
            {
                return Result.Failure<MessageDeletedForEveryoneResponse>(MessageErrors.NotFound);
            }

            // Check if user is the sender of the message
            if (message.User?.Id != userId)
            {
                return Result.Failure<MessageDeletedForEveryoneResponse>(MessageErrors.UnauthorizedToDeleteForEveryone);
            }

            // Delete message for everyone
            var deleteResult = message.DeleteForEveryone();

            if (deleteResult.IsFailure)
            {
                return Result.Failure<MessageDeletedForEveryoneResponse>(deleteResult.Error);
            }

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var participantsIds = chat.Participants.Select(p => p.Id.Value).ToList();

            return new MessageDeletedForEveryoneResponse(
                participantsIds,
                command.ChatId,
                command.MessageId);
        }
    }
}
