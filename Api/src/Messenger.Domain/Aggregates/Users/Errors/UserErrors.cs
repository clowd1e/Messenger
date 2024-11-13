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
    }
}
