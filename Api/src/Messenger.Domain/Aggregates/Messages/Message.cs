using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Messages.ValueObjects;
using Messenger.Domain.Aggregates.Messages.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Messages
{
    public sealed class Message : AggregateRoot<MessageId>
    {
        private MessageTimestamp _timestamp;
        private MessageContent _content;

        private Message() 
            : base(new(Guid.NewGuid())) { }

        private Message(
            MessageId messageId,
            MessageTimestamp timestamp,
            MessageContent content) : base(messageId)
        {
            Timestamp = timestamp;
            Content = content;
        }

        public MessageTimestamp Timestamp
        {
            get => _timestamp;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _timestamp = value;
            }
        }

        public MessageContent Content
        {
            get => _content;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _content = value;
            }
        }

        public Chat? Chat { get; private set; } = default;
        public Users.User? User { get; private set; } = default;

        public Result SetChat(Chat chat)
        {
            ArgumentNullException.ThrowIfNull(chat);

            Chat = chat;

            return Result.Success();
        }

        public Result SetUser(Users.User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            User = user;

            return Result.Success();
        }

        public static Result<Message> Create(
            MessageId messageId,
            MessageTimestamp timestamp,
            MessageContent content)
        {
            return new Message(
                messageId,
                timestamp,
                content);
        }
    }
}
