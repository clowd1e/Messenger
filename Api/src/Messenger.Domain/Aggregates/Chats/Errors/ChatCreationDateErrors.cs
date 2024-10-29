using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats.Errors
{
    public static class ChatCreationDateErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "ChatCreationDate.Empty",
                description: "Chat creation date cannot be empty.");

        public static Error InvalidKind(DateTimeKind kind) =>
            Error.Validation(
                code: "ChatCreationDate.InvalidKind",
                description: $"Chat creation date kind must be in {kind} format.");

        public static readonly Error FutureDate =
            Error.Validation(
                code: "ChatCreationDate.FutureDate",
                description: "Chat creation date cannot be in the future.");
    }
}
