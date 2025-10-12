using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Common.Timestamp
{
    public sealed class Timestamp : ValueObject, IComparable<Timestamp>
    {
        private Timestamp(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        public static Result<Timestamp> Create(DateTime value)
        {
            return new Timestamp(value);
        }

        public static Timestamp UtcNow()
        {
            return new Timestamp(DateTime.UtcNow);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public int CompareTo(Timestamp? other)
        {
            if (other is null)
                return 1;

            return Value.CompareTo(other.Value);
        }

        public static bool operator >(Timestamp left, Timestamp right) => left.CompareTo(right) > 0;
        public static bool operator <(Timestamp left, Timestamp right) => left.CompareTo(right) < 0;
        public static bool operator >=(Timestamp left, Timestamp right) => left.CompareTo(right) >= 0;
        public static bool operator <=(Timestamp left, Timestamp right) => left.CompareTo(right) <= 0;
    }
}
