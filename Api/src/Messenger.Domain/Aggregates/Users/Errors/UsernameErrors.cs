using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Users.Errors
{
    public static class UsernameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "Username.Empty",
                description: "Username cannot be empty.");

        public static Error TooShort(int minLength) =>
            Error.Validation(
                code: "Username.TooShort",
                description: $"Username must be at least {minLength} characters long.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "Username.TooLong",
                description: $"Username must be less than {maxLength} characters long.");
    }
}
