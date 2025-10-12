using System.ComponentModel.DataAnnotations;

namespace Messenger.Application.Identity.Options
{
    public sealed class RefreshTokenSettings
    {
        public const string SectionName = nameof(RefreshTokenSettings);

        [Required]
        [Range(1, 20)]
        public int ExpirationTimeInDays { get; init; }
    }
}
