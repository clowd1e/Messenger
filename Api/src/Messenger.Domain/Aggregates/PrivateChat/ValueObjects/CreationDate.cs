using Messenger.Domain.Aggregates.PrivateChat.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.PrivateChat.ValueObjects
{
    public sealed class CreationDate : ValueObject
    {
        public const DateTimeKind Kind = DateTimeKind.Utc;

        private CreationDate(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        public static Result<CreationDate> Create(DateTime creationDate)
        {
            if (creationDate == default)
            {
                return Result.Failure<CreationDate>(CreationDateErrors.Empty);
            }

            if (creationDate.Kind != Kind)
            {
                return Result.Failure<CreationDate>(
                    CreationDateErrors.InvalidKind(Kind));
            }

            if (creationDate > DateTime.UtcNow)
            {
                return Result.Failure<CreationDate>(CreationDateErrors.FutureDate);
            }

            return new CreationDate(creationDate);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
