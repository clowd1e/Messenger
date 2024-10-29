using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats.ValueObjects
{
    public sealed class ChatCreationDate : ValueObject
    {
        public const DateTimeKind Kind = DateTimeKind.Utc;

        private ChatCreationDate(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        public static Result<ChatCreationDate> Create(DateTime creationDate)
        {
            if (creationDate == default)
            {
                return Result.Failure<ChatCreationDate>(
                    ChatCreationDateErrors.Empty);
            }

            if (creationDate.Kind != Kind)
            {
                return Result.Failure<ChatCreationDate>(
                    ChatCreationDateErrors.InvalidKind(Kind));
            }

            if (creationDate > DateTime.UtcNow)
            {
                return Result.Failure<ChatCreationDate>(
                    ChatCreationDateErrors.FutureDate);
            }

            return new ChatCreationDate(creationDate);
        }

        public static Result<ChatCreationDate> UtcNow()
        {
            return new ChatCreationDate(DateTime.UtcNow);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
