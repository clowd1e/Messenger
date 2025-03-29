using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Users.Errors
{
    public static class RegistrationDateErrors
    {
        public static Error FutureDate =
            Error.Validation(
                code: "RegistrationDate.FutureDate",
                description: "Registration date cannot be in the future.");
    }
}
