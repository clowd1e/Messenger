namespace Messenger.Application.Storage.Blobs.DTO
{
    public sealed record DownloadBlobResponse(
        Stream Stream,
        string ContentType);
}
