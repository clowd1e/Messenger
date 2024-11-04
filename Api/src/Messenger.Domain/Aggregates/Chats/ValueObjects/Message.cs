using Messenger.Domain.Aggregates.Chats.Messages.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects
{
    public sealed class Message : ValueObject
    {
        private UserId _userId;
        private MessageTimestamp _timestamp;
        private MessageContent _content;

        private Message(
            MessageTimestamp timestamp,
            MessageContent content,
            UserId userId)
        {
            Timestamp = timestamp;
            Content = content;
            UserId = userId;
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

        public UserId UserId
        {
            get => _userId;
            init => _userId = value;
        }

        public static Result<Message> Create(
            MessageTimestamp timestamp,
            MessageContent content,
            UserId userId)
        {
            return new Message(
                timestamp,
                content,
                userId);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
            yield return _timestamp;
            yield return _content;
        }
    }
}
