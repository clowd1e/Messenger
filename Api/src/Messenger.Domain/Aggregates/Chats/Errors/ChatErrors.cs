using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats.Errors
{
    public static class ChatErrors
    {
        public static Error ChatWithMoreThanTwoUsers(int usersCount) =>
            Error.Validation(
                code: "Chat.ChatWithMoreThanTwoUsers",
                description: $"Chat cannot have more than {usersCount} users.");
    }
}
