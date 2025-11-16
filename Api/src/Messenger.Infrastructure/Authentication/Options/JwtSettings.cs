using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Infrastructure.Authentication.Options
{
    public sealed class JwtSettings
    {
        public const string SectionName = nameof(JwtSettings);

        [Required]
        [StringLength(200)]
        public required string Issuer { get; set; }

        [Required]
        [StringLength(200)]
        public required string Audience { get; set; }

        [Required]
        public required string SecretKey { get; set; }

        [Range(1, 1000)]
        public int ExpirationTimeInMinutes { get; set; } = 30;

        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;
    }
}
