using Messenger.Domain.Aggregates.Messages.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats.Messages.ValueObjects
{
    public sealed class MessageContent : ValueObject
    {
        public const int MaxLength = 1000;

        private MessageContent(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<MessageContent> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<MessageContent>(
                    MessageContentErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<MessageContent>(
                    MessageContentErrors.TooLong(MaxLength));
            }

            return new MessageContent(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
