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

        public readonly static Error UserNotInChat =
            Error.Validation(
                code: "Chat.UserNotInChat",
                description: "User is not in chat.");

        public readonly static Error ChatWithSameUser =
            Error.Validation(
                code: "Chat.ChatWithSameUser",
                description: "Chat cannot be created with the same user.");

        public readonly static Error ChatAlreadyExists =
            Error.Validation(
                code: "Chat.ChatAlreadyExists",
                description: "Chat already exists.");
    }
}
