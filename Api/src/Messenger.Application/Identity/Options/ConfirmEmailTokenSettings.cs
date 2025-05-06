using System.ComponentModel.DataAnnotations;

namespace Messenger.Application.Identity.Options
{
    public sealed class ConfirmEmailTokenSettings
    {
        public const string SectionName = nameof(ConfirmEmailTokenSettings);

        [Required]
        [Range(1, 21)]
        public int ExpirationTimeInDays { get; set; }

        // Note: will be used in ResendConfirmationEmailCommand
        [Required]
        [Range(1, 20)]
        public int ActiveTokensLimit { get; set; }
    }
}
