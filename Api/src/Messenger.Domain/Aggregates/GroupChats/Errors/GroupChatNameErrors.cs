using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.GroupChats.Errors
{
    public static class GroupChatNameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "GroupChatName.Empty",
                description: "The group chat's name cannot be empty.");

        public static Error TooShort(int minLength) =>
            Error.Validation(
                code: "GroupChatName.TooShort",
                description: $"The group chat's name must be at least {minLength} characters long.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "GroupChatName.TooLong",
                description: $"The group chat's name must be less than {maxLength} characters long.");
    }
}
