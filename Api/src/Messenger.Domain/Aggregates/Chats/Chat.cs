using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats
{
    public sealed class Chat : AggregateRoot<ChatId>
    {
        public const int UsersCount = 2;

        private readonly HashSet<Message> _messages = [];
        private readonly HashSet<Users.User> _users = [];
        private ChatCreationDate _creationDate;

        private Chat()
            : base(new(Guid.NewGuid())) { }

        private Chat(
            ChatId chatId,
            ChatCreationDate creationDate,
            List<Users.User> users) : base(chatId)
        {
            CreationDate = creationDate;
            _users = [.. users];
        }

        public ChatCreationDate CreationDate
        {
            get => _creationDate;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _creationDate = value;
            }
        }

        public IReadOnlyCollection<Message> Messages => _messages;
        public IReadOnlyCollection<Users.User> Users => _users;

        public Result AddMessage(Message message)
        {
            if (!Users.Select(u => u.Id).Contains(message.UserId))
            {
                return ChatErrors.UserNotInChat;
            }

            _messages.Add(message);

            return Result.Success();
        }

        public static Result<Chat> Create(
            ChatId chatId,
            ChatCreationDate creationDate,
            List<Users.User> users)
        {
            if (users.Count != UsersCount)
            {
                return Result.Failure<Chat>(
                    ChatErrors.ChatWithMoreThanTwoUsers(UsersCount));
            }

            var chat = new Chat(
                chatId,
                creationDate,
                users);

            users.ForEach(u => u.AddChat(chat));

            return chat;
        }
    }
}
