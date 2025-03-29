using System.ComponentModel.DataAnnotations;

namespace Messenger.Infrastructure.Services.Emails.Options
{
    public sealed class EmailLinkGeneratorSettings
    {
        public const string SectionName = nameof(EmailLinkGeneratorSettings);

        [Required]
        [StringLength(100)]
        [Url]
        public string ClientAppUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailConfirmationPath { get; set; }
    }
}
