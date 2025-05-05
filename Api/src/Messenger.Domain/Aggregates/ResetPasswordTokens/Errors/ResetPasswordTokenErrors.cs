using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.ResetPasswordTokens.Errors
{
    public static class ResetPasswordTokenErrors
    {
        public static readonly Error AlreadyUsed =
            Error.Validation(
                code: "ResetPasswordToken.AlreadyUsed",
                description: "Reset password token is already used.");

        public static readonly Error NotFound =
            Error.NotFound(
                code: "ResetPasswordToken.NotFound",
                description: "Reset password token not found.");

        public static readonly Error TooManyActiveTokens =
            Error.Validation(
                code: "ResetPasswordToken.TooManyActiveTokens",
                description: "User has exceeded the maximum number of active reset password tokens.");

        public static readonly Error TokenNotAssignedToUser =
            Error.Validation(
                code: "ResetPasswordToken.TokenNotAssignedToUser",
                description: "Reset password token is not assigned to the user.");

        public static readonly Error Expired =
            Error.Validation(
                code: "ResetPasswordToken.Expired",
                description: "Reset password token has expired.");

        public static readonly Error Invalid =
            Error.Validation(
                code: "ResetPasswordToken.Invalid",
                description: "The provided reset password token is invalid.");
    }
}
