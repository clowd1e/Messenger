using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Common.Timestamp
{
    public sealed class Timestamp : ValueObject
    {
        private Timestamp(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        public static Result<Timestamp> CreateLessThanOrEqualUtcNow(DateTime value)
        {
            if (value > DateTime.UtcNow)
            {
                return Result.Failure<Timestamp>(TimestampErrors.FutureDate);
            }

            return new Timestamp(value);
        }

        public static Result<Timestamp> Create(DateTime value)
        {
            return new Timestamp(value);
        }

        public static Result<Timestamp> UtcNow()
        {
            return new Timestamp(DateTime.UtcNow);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static bool operator >(Timestamp left, Timestamp right) => left.Value > right.Value;
        public static bool operator <(Timestamp left, Timestamp right) => left.Value < right.Value;
        public static bool operator >=(Timestamp left, Timestamp right) => left.Value >= right.Value;
        public static bool operator <=(Timestamp left, Timestamp right) => left.Value <= right.Value;
    }

    public static class TimestampErrors
    {
        public static readonly Error FutureDate =
            Error.Validation(
                code: "Timestamp.FutureDate",
                description: "The timestamp cannot be in the future compared to UTC now.");
    }
}
