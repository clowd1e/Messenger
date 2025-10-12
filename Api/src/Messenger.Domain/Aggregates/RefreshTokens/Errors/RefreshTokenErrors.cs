using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.RefreshTokens.Errors
{
    public static class RefreshTokenErrors
    {
        public static readonly Error InvalidExpiryTime =
            Error.Validation(
                code: "RefreshToken.InvalidExpiryTime",
                description: "The expiry time of the refresh token must be in the future.");

        public static readonly Error TokenExpired =
            Error.Validation(
                code: "RefreshToken.TokenExpired",
                description: "The provided refresh token has expired.");

        public static readonly Error NotFound =
            Error.Validation(
                code: "RefreshToken.NotFound",
                description: "The refresh token was not found.");


        public static readonly Error InvalidToken =
            Error.Validation(
                code: "RefreshToken.InvalidToken",
                description: "The provided refresh token is invalid.");
    }
}
