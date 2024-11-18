using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats.Errors
{
    public static class ChatErrors
    {
        public readonly static Error NotFound =
            Error.NotFound(
                code: "Chat.NotFound",
                description: "Chat not found.");

        public static Error ChatWithMoreThanTwoUsers(int usersCount) =>
            Error.Validation(
                code: "Chat.ChatWithMoreThanTwoUsers",
                description: $"Chat cannot have more than {usersCount} users.");
    }
}
