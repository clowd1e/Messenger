using System.ComponentModel.DataAnnotations;

namespace Messenger.Infrastructure.External.Blobs.Options
{
    public sealed class AzureBlobStorageSettings
    {
        public const string SectionName = nameof(AzureBlobStorageSettings);

        [Required]
        [StringLength(50)]
        public string ContainerName { get; init; }

        [Required]
        public string ConnectionString { get; init; }

        [Required]
        [Url]
        public string ImagesContainerUri { get; init; }
    }
}
