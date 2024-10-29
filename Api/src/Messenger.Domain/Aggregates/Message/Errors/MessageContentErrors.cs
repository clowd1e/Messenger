using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Message.Errors
{
    public static class MessageContentErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "MessageContent.Empty",
                description: "Message content cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "MessageContent.TooLong",
                description: $"Message content must be less than {maxLength} characters long.");
    }
}
