using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Messages.Errors
{
    public static class MessageErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Message.NotFound",
                description: "Message not found.");

        public static readonly Error MessageAlreadyDeletedForEveryone =
            Error.Validation(
                code: "Message.AlreadyDeletedForEveryone",
                description: "This message has been deleted for everyone and cannot be accessed.");

        public static readonly Error MessageAlreadyDeletedForUser =
            Error.Validation(
                code: "Message.AlreadyDeletedForUser",
                description: "This message has already been deleted for this user.");

        public static readonly Error UnauthorizedToDeleteForEveryone =
            Error.Validation(
                code: "Message.UnauthorizedToDeleteForEveryone",
                description: "Only the message sender can delete this message for everyone.");

        public static readonly Error UnauthorizedToUpdate =
            Error.Validation(
                code: "Message.UnauthorizedToUpdate",
                description: "Only the message sender can update this message.");
    }
}