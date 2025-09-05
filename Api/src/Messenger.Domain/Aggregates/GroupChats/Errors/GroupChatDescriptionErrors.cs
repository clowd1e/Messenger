using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.GroupChats.Errors
{
    public static class GroupChatDescriptionErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "GroupChatDescription.Empty",
                description: "The group chat's description cannot be empty.");

        public static Error TooShort(int minLength) =>
            Error.Validation(
                code: "GroupChatDescription.TooShort",
                description: $"The group chat's description chat must be at least {minLength} characters long.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "GroupChatDescription.TooLong",
                description: $"The group chat's description chat must be less than {maxLength} characters long.");
    }
}
