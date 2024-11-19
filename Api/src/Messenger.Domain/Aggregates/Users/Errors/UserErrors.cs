using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.User.Errors
{
    public static class UserErrors
    {
        public static readonly Error NotFound =
            Error.Validation(
                code: "User.NotFound",
                description: "User not found.");

        public static readonly Error UserAlreadyHasChat =
            Error.Validation(
                code: "User.UserAlreadyHasChat",
                description: "User already has this chat.");

        public static readonly Error InvalidAccessToken =
            Error.Validation(
                code: "User.InvalidAccessToken",
                description: "The provided access token is invalid.");

        public static readonly Error InvalidRefreshToken =
            Error.Validation(
                code: "User.InvalidRefreshToken",
                description: "The provided refresh token is invalid.");

        public static readonly Error RefreshTokenExpired =
            Error.Validation(
                code: "User.RefreshTokenExpired",
                description: "The provided refresh token has expired.");

        public static readonly Error InvalidCredentials =
            Error.Validation(
                code: "User.InvalidCredentials",
                description: "The provided credentials are invalid.");
    }
}
