using System.ComponentModel.DataAnnotations;

namespace Messenger.Infrastructure.Authentication.Options
{
    public sealed class LoginSettings
    {
        public const string SectionName = nameof(LoginSettings);

        public bool IsPersistent { get; set; } = false;

        public bool LockoutOnFailure { get; set; } = true;

        [Required]
        [Range(1, 100)]
        public int? RefreshTokenExpiryInDays { get; set; }
    }
}
