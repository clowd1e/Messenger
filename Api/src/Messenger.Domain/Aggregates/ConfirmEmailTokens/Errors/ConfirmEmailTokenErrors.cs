using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.ConfirmEmailTokens.Errors
{
    public static class ConfirmEmailTokenErrors
    {
        public static readonly Error AlreadyUsed =
            Error.Validation(
                code: "ConfirmEmailToken.AlreadyUsed",
                description: "Confirm email token is already used.");

        public static readonly Error NotFound =
            Error.NotFound(
                code: "ConfirmEmailToken.NotFound",
                description: "Confirm email token not found.");

        public static readonly Error TooManyActiveTokens =
            Error.Validation(
                code: "ConfirmEmailToken.TooManyActiveTokens",
                description: "User has exceeded the maximum number of active confirm email tokens.");

        public static readonly Error TokenNotAssignedToUser =
            Error.Validation(
                code: "ConfirmEmailToken.TokenNotAssignedToUser",
                description: "Confirm email token is not assigned to the user.");

        public static readonly Error Expired =
            Error.Validation(
                code: "ConfirmEmailToken.Expired",
                description: "Confirm email token has expired.");

        public static readonly Error Invalid =
            Error.Validation(
                code: "ConfirmEmailToken.Invalid",
                description: "The provided confirm email token is invalid.");
    }
}
