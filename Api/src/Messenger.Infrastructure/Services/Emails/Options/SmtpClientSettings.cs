using System.ComponentModel.DataAnnotations;

namespace Messenger.Infrastructure.Services.Emails.Options
{
    public sealed class SmtpClientSettings
    {
        public const string SectionName = nameof(SmtpClientSettings);

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Username { get; set; }

        [StringLength(100)]
        public string? Password { get; set; }

        [Required]
        [StringLength(100)]
        public required string Host { get; set; }

        [Required]
        [Range(1, 65535)]
        public int Port { get; set; }

        [Required]
        public bool EnableSsl { get; set; }
    }
}
