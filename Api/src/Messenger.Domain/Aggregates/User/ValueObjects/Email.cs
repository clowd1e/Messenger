using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.User.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public const int MaxLength = 50;

        private Email(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<Email>(EmailErrors.Empty);
            }

            if (email.Length > MaxLength)
            {
                return Result.Failure<Email>(EmailErrors.TooLong(MaxLength));
            }

            return new Email(email);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
