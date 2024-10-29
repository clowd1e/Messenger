using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.PrivateChat.Errors
{
    public static class PrivateChatCreationDateErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "PrivateChatCreationDate.Empty",
                description: "Private chat creation date cannot be empty.");

        public static Error InvalidKind(DateTimeKind kind) =>
            Error.Validation(
                code: "PrivateChatCreationDate.InvalidKind",
                description: $"Private chat creation date kind must be in {kind} format.");

        public static readonly Error FutureDate =
            Error.Validation(
                code: "PrivateChatCreationDate.FutureDate",
                description: "Private chat creation date cannot be in the future.");
    }
}
