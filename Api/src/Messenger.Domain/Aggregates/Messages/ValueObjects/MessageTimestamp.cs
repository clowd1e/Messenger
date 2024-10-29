using Messenger.Domain.Aggregates.Messages.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Messages.ValueObjects
{
    public sealed class MessageTimestamp : ValueObject
    {
        public const DateTimeKind Kind = DateTimeKind.Utc;

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

            if (timestamp.Kind != Kind)
            {
                return Result.Failure<MessageTimestamp>(
                    MessageTimestampErrors.InvalidKind(Kind));
            }

            if (timestamp > DateTime.UtcNow)
            {
                return Result.Failure<MessageTimestamp>(
                    MessageTimestampErrors.FutureDate);
            }

            return new MessageTimestamp(timestamp);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
