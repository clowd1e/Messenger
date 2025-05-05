using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Common.TokenHash
{
    public sealed class TokenHash : ValueObject
    {
        public const int MaxLength = 256;

        private TokenHash(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<TokenHash> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<TokenHash>(
                    TokenHashErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<TokenHash>(
                    TokenHashErrors.TooLong(MaxLength));
            }

            return new TokenHash(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
