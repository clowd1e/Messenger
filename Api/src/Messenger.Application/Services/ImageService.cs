using Messenger.Application.Abstractions.Storage;
using Messenger.Application.Images.Options;
using Messenger.Domain.Aggregates.Common.ImageUri;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Services
{
    public sealed class ImageService : IImageService
    {
        private readonly IBlobService _blobService;
        private readonly ImageSettings _imageSettings;

        public ImageService(
            IBlobService blobService,
            IOptions<ImageSettings> imageSettings)
        {
            _blobService = blobService;
            _imageSettings = imageSettings.Value;
        }


        public async Task DeleteImageAsync(
            ImageUri imageUri,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(imageUri);

            await _blobService.DeleteAsync(
                imageUri.Value,
                cancellationToken);
        }

        public async Task<ImageUri> UploadImageAsync(
            IFormFile image,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(image);

            if (!ValidImage(image))
            {
                throw new InvalidOperationException("Invalid image");
            }

            using Stream stream = image.OpenReadStream();

            var uploadResponse = await _blobService.UploadAsync(
                stream,
                image.ContentType,
                cancellationToken);

            var imageUriResult = ImageUri.Create(uploadResponse.BlobUri);

            if (imageUriResult.IsFailure)
            {
                throw new InvalidOperationException($"Failed to create image uri: {imageUriResult.Error}");
            }

            return imageUriResult.Value;
        }

        private bool ValidImage(IFormFile image) =>
            _imageSettings.AllowedContentTypes.Contains(image.ContentType);
    }
}
