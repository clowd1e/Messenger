using Messenger.Application.Storage.Blobs.DTO;

namespace Messenger.Application.Abstractions.Storage
{
    public interface IBlobService
    {
        Task<UploadBlobResponse> UploadAsync(
            Stream stream,
            string contentType,
            string? subDirectory = null,
            CancellationToken cancellationToken = default);

        Task<DownloadBlobResponse> DownloadAsync(
            string blobName,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            string blobUri,
            CancellationToken cancellationToken = default);
    }
}
