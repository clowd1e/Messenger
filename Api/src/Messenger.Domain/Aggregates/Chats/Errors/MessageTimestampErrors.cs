using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Messages.Errors
{
    public static class MessageTimestampErrors
    {
        public static Error Empty =
            Error.Validation(
                code: "MessageTimestamp.Empty",
                description: "Message timestamp cannot be empty.");

        public static Error FutureDate =
            Error.Validation(
                code: "MessageTimestamp.FutureDate",
                description: "Message timestamp cannot be in the future.");
    }
}
