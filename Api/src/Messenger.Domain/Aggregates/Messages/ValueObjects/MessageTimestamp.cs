using Messenger.Domain.Aggregates.Messages.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats.Messages.ValueObjects
{
    public sealed class MessageTimestamp : ValueObject
    {
        private MessageTimestamp(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        public static Result<MessageTimestamp> Create(DateTime timestamp)
        {
            if (timestamp == default)
            {
                return Result.Failure<MessageTimestamp>(
                    MessageTimestampErrors.Empty);
            }

            if (timestamp > DateTime.UtcNow)
            {
                return Result.Failure<MessageTimestamp>(
                    MessageTimestampErrors.FutureDate);
            }

            return new MessageTimestamp(timestamp);
        }

        public static Result<MessageTimestamp> UtcNow()
        {
            return new MessageTimestamp(DateTime.UtcNow);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
