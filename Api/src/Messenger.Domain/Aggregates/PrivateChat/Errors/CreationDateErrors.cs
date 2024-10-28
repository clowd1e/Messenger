using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.PrivateChat.Errors
{
    public static class CreationDateErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "CreationDate.Empty",
                description: "Creation date cannot be empty.");

        public static Error InvalidKind(DateTimeKind kind) =>
            Error.Validation(
                code: "CreationDate.InvalidKind",
                description: $"Creation date kind must be in {kind} format.");

        public static readonly Error FutureDate =
            Error.Validation(
                code: "CreationDate.FutureDate",
                description: "Creation date cannot be in the future.");
    }
}
