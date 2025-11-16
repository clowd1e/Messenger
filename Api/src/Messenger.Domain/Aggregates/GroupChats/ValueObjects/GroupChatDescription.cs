using Messenger.Domain.Aggregates.GroupChats.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.GroupChats.ValueObjects
{
    public sealed class GroupChatDescription : ValueObject
    {
        public const int MinLength = 1;
        public const int MaxLength = 200;

        private GroupChatDescription(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<GroupChatDescription> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<GroupChatDescription>(
                    GroupChatDescriptionErrors.Empty);
            }

            if (value.Length < MinLength)
            {
                return Result.Failure<GroupChatDescription>(
                    GroupChatDescriptionErrors.TooShort(MinLength));
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<GroupChatDescription>(
                    GroupChatDescriptionErrors.TooLong(MaxLength));
            }

            return new GroupChatDescription(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
