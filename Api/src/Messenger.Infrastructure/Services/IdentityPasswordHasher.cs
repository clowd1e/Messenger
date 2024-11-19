using Messenger.Application.Identity;
using Messenger.Infrastructure.Services.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Services
{
    public sealed class IdentityPasswordHasher : IPasswordHasher<ApplicationUser>
    {
        private readonly HashSettings _hashSettings;

        public IdentityPasswordHasher(IOptions<HashSettings> hashSettings)
        {
            _hashSettings = hashSettings.Value;
        }

        public string HashPassword(
            ApplicationUser user,
            string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(
                password,
                _hashSettings.WorkFactor.Value);

            return passwordHash;
        }

        public PasswordVerificationResult VerifyHashedPassword(
            ApplicationUser user,
            string hashedPassword,
            string providedPassword)
        {
            var passwordVerificationResult = BCrypt.Net.BCrypt.EnhancedVerify(
                providedPassword,
                hashedPassword);

            return passwordVerificationResult
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
    }
}
