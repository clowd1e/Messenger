using System.ComponentModel.DataAnnotations;

namespace Messenger.Application.Images.Options
{
    public sealed class ImageSettings
    {
        public const string SectionName = nameof(ImageSettings);

        [Required]
        public HashSet<string> AllowedContentTypes { get; init; }
    }
}
