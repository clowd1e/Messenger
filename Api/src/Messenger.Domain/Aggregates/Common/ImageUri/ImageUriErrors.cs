using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Common.ImageUri
{
    public static class ImageUriErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "ImageUri.Empty",
                description: "Image uri cannot be empty.");

        public static readonly Error InvalidUri =
            Error.Validation(
                code: "ImageUri.InvalidUri",
                description: "Provided image uri is invalid.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "ImageUri.TooLong",
                description: $"Image uri text must not exceed {allowedLength} characters.");
    }
}
