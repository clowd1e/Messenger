using Messenger.Domain.Aggregates.Common.ImageUri;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Abstractions.Storage
{
    public interface IImageService
    {
        Task<ImageUri> UploadImageAsync(
            IFormFile image,
            string? subDirectory = null,
            CancellationToken cancellationToken = default);

        Task DeleteImageAsync(
            ImageUri imageUri,
            CancellationToken cancellationToken = default);
    }
}
