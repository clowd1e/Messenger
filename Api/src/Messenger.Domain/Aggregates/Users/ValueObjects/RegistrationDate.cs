using Messenger.Domain.Aggregates.Users.Errors;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Users.ValueObjects
{
    public sealed class RegistrationDate : ValueObject
    {
        private RegistrationDate(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        public static Result<RegistrationDate> Create(DateTime registrationDate)
        {
            if (registrationDate > DateTime.UtcNow)
            {
                return Result.Failure<RegistrationDate>(
                    RegistrationDateErrors.FutureDate);
            }

            return new RegistrationDate(registrationDate);
        }

        public static Result<RegistrationDate> UtcNow()
        {
            return new RegistrationDate(DateTime.UtcNow);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static explicit operator DateTime(RegistrationDate registrationDate) => registrationDate.Value;
    }
}
