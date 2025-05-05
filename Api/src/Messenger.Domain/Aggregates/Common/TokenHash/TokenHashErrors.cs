using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Common.TokenHash
{
    public static class TokenHashErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "TokenHash.Empty",
                description: "Token hash cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "TokenHash.TooLong",
                description: $"Token hash cannot be longer than {maxLength} characters.");
    }
}
