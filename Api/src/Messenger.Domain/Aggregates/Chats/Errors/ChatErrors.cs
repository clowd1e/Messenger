using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats.Errors
{
    public static class ChatErrors
    {
        public readonly static Error NotFound =
            Error.NotFound(
                code: "Chat.NotFound",
                description: "Chat not found.");

        public readonly static Error UserNotInChat =
            Error.Validation(
                code: "Chat.UserNotInChat",
                description: "User is not in chat.");

        public static readonly Error UserAlreadyInChat =
            Error.Validation(
                code: "Chat.UserAlreadyHasChat",
                description: "User is already in chat.");

        public readonly static Error InvalidGroupMember =
            Error.Validation(
                code: "Chat.InvalidGroupMember",
                description: "Group member is invalid.");

        public readonly static Error ChatWithSameUser =
            Error.Validation(
                code: "Chat.ChatWithSameUser",
                description: "Chat cannot be created with the same user.");

        public readonly static Error ChatAlreadyExists =
            Error.Validation(
                code: "Chat.ChatAlreadyExists",
                description: "Chat already exists.");

        public static readonly Error InvalidChatType =
            Error.Validation(
                code: "Chat.InvalidChatType",
                description: "Chat is not of the expected type.");

        public static Error MaxParticipantsLimit(int participantsCount) =>
            Error.Validation(
                code: "Chat.MaxParticipantsLimit",
                description: $"Chat cannot have more than {participantsCount} users.");

        public static Error MinParticipantsLimit(int participantsCount) =>
            Error.Validation(
                code: "Chat.MinParticipantsLimit",
                description: $"Chat must have at least {participantsCount} participants.");

        public static Error InvalidParticipantsCount(int expectedCount) =>
            Error.Validation(
                code: "Chat.InvalidParticipantsCount",
                description: $"Chat must have exactly {expectedCount} participants.");
    }
}
