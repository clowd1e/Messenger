using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats
{
    public sealed class PrivateChat : Chat
    {
        public const int UsersCount = 2;

        private PrivateChat()
            : base() { }

        private PrivateChat(
            ChatId chatId,
            Timestamp creationDate)
            : base(chatId, creationDate)
        { }

        public static Result<PrivateChat> Create(
            ChatId chatId,
            Timestamp creationDate,
            List<Users.User> participants)
        {
            if (participants.Count != UsersCount)
            {
                return Result.Failure<PrivateChat>(
                    ChatErrors.InvalidParticipantsCount(UsersCount));
            }

            var privateChat = new PrivateChat(chatId, creationDate);

            privateChat.SetParticipants(participants);

            return Result.Success(privateChat);
        }
    }
}
