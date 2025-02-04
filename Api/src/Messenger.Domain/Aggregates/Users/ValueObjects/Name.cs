using Messenger.Domain.Aggregates.Users.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Users.ValueObjects
{
    public sealed class Name : ValueObject
    {
        public const int MaxLength = 30;

        private Name(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Name> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Name>(NameErrors.Empty);
            }

            if (value!.Length > MaxLength)
            {
                return Result.Failure<Name>(NameErrors.TooLong(MaxLength));
            }

            return new Name(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
