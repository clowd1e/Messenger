using Microsoft.AspNetCore.Identity;

namespace Messenger.Application.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string? RefreshTokenHash { get; set; }
        public DateTime RefreshTokenExpirationTime { get; set; }
    }
}
