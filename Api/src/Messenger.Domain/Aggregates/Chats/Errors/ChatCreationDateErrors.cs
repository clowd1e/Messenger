using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats.Errors
{
    public static class ChatCreationDateErrors
    {
        public static readonly Error FutureDate =
            Error.Validation(
                code: "ChatCreationDate.FutureDate",
                description: "Chat creation date cannot be in the future.");
    }
}
