using System.ComponentModel.DataAnnotations;

namespace Messenger.Application.Identity.Options
{
    public sealed class LoginSettings
    {
        public const string SectionName = nameof(LoginSettings);

        public bool IsPersistent { get; set; } = false;

        public bool LockoutOnFailure { get; set; } = true;

        [Required]
        [Range(1, 20)]
        public int MaxSessionsCount { get; init; }
    }
}
