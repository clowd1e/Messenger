using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Messenger.Application.Abstractions.Storage;
using Messenger.Application.Storage.Blobs.DTO;
using Messenger.Infrastructure.Exceptions;
using Messenger.Infrastructure.External.Blobs.Options;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.External.Blobs
{
    public sealed class AzureBlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly AzureBlobStorageSettings _storageSettings;

        public AzureBlobService(
            BlobServiceClient blobServiceClient,
            IOptions<AzureBlobStorageSettings> storageSettings)
        {
            _storageSettings = storageSettings.Value;

            _containerClient = blobServiceClient.GetBlobContainerClient(
                _storageSettings.ContainerName);
        }

        public async Task DeleteAsync(
            string blobUri,
            CancellationToken cancellationToken = default)
        {
            var blobName = Path.GetFileName(blobUri);

            var isSuccess = await _containerClient.DeleteBlobIfExistsAsync(
                blobName,
                cancellationToken: cancellationToken);

            if (!isSuccess)
            {
                throw new InvalidOperationException("Failed to delete blob from the storage.");
            }
        }

        public async Task<DownloadBlobResponse> DownloadAsync(
            string blobName,
            CancellationToken cancellationToken = default)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            var blobExists = await blobClient.ExistsAsync(cancellationToken);

            if (!blobExists)
            {
                throw new BlobNotFoundException(blobName);
            }

            var response = await blobClient.DownloadAsync(cancellationToken);

            return new DownloadBlobResponse(
                response.Value.Content,
                response.Value.ContentType);
        }

        public async Task<UploadBlobResponse> UploadAsync(
            Stream stream,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            var blobName = Guid.NewGuid().ToString();

            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(
                stream,
                new BlobHttpHeaders { ContentType = contentType },
                cancellationToken: cancellationToken);

            return new UploadBlobResponse(
                new Uri(_storageSettings.ImagesContainerUri),
                blobName);
        }
    }
}
