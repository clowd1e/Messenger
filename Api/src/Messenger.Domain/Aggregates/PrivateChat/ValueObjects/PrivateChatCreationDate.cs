using Messenger.Domain.Aggregates.PrivateChat.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.PrivateChat.ValueObjects
{
    public sealed class PrivateChatCreationDate : ValueObject
    {
        public const DateTimeKind Kind = DateTimeKind.Utc;

        private PrivateChatCreationDate(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        public static Result<PrivateChatCreationDate> Create(DateTime creationDate)
        {
            if (creationDate == default)
            {
                return Result.Failure<PrivateChatCreationDate>(
                    PrivateChatCreationDateErrors.Empty);
            }

            if (creationDate.Kind != Kind)
            {
                return Result.Failure<PrivateChatCreationDate>(
                    PrivateChatCreationDateErrors.InvalidKind(Kind));
            }

            if (creationDate > DateTime.UtcNow)
            {
                return Result.Failure<PrivateChatCreationDate>(
                    PrivateChatCreationDateErrors.FutureDate);
            }

            return new PrivateChatCreationDate(creationDate);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
