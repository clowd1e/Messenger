using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Users.Errors
{
    public static class NameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "Name.Empty",
                description: "Name cannot be empty.");

        public static Error TooShort(int minLength) =>
            Error.Validation(
                code: "Name.TooShort",
                description: $"Name must be at least {minLength} characters long.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "Name.TooLong",
                description: $"Name must be less than {maxLength} characters long.");
    }
}
