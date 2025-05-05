using System.ComponentModel.DataAnnotations;

namespace Messenger.Application.Identity.Options
{
    public sealed class ResetPasswordTokenSettings
    {
        public const string SectionName = nameof(ResetPasswordTokenSettings);

        [Required]
        [Range(1, 12)]
        public int ExpirationTimeInHours { get; set; }

        [Required]
        [Range(1, 20)]
        public int ActiveTokensLimit { get; set; }
    }
}
