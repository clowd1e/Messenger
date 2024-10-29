using Messenger.Domain.Aggregates.Messages.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Messages
{
    public sealed class Message : AggregateRoot<MessageId>
    {
        private readonly Users.User _user;
        private MessageTimestamp _timestamp;
        private MessageContent _content;

        private Message()
            : base(new(Guid.NewGuid())) { }

        private Message(
            MessageId messageId,
            MessageTimestamp timestamp,
            MessageContent content,
            Users.User user) : base(messageId)
        {
            Timestamp = timestamp;
            Content = content;
            User = user;
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

        public Users.User User
        {
            get => _user;
            init => _user = value;
        }

        public static Result<Message> Create(
            MessageId messageId,
            MessageTimestamp timestamp,
            MessageContent content,
            Users.User user)
        {
            return new Message(
                messageId,
                timestamp,
                content,
                user);
        }
    }
}
