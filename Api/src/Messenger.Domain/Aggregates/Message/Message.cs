using Messenger.Domain.Aggregates.Message.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Message
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
