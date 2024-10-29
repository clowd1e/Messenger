using Messenger.Domain.Aggregates.Users.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Users.ValueObjects
{
    public sealed class Username : ValueObject
    {
        public const int MaxLength = 35;

        private Username(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Username> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Username>(UsernameErrors.Empty);
            }

            if (value!.Length > MaxLength)
            {
                return Result.Failure<Username>(UsernameErrors.TooLong(MaxLength));
            }

            return new Username(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
