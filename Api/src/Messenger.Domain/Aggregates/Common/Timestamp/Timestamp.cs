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
    }
}
