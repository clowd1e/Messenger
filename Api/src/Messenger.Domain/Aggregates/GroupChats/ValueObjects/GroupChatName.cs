using Messenger.Domain.Aggregates.GroupChats.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.GroupChats.ValueObjects
{
    public sealed class GroupChatName : ValueObject
    {
        public const int MinLength = 3;
        public const int MaxLength = 50;
        
        private GroupChatName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<GroupChatName> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<GroupChatName>(
                    GroupChatNameErrors.Empty);
            }

            if (value.Length < MinLength)
            {
                return Result.Failure<GroupChatName>(
                    GroupChatNameErrors.TooShort(MinLength));
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<GroupChatName>(
                    GroupChatNameErrors.TooLong(MaxLength));
            }

            return new GroupChatName(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
