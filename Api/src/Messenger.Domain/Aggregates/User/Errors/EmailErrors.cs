using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.User.Errors
{
    public static class EmailErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "Email.Empty",
                description: "Email cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "Email.TooLong",
                description: $"Email must be less than {maxLength} characters long.");
    }
}
