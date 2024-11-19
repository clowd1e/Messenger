using Messenger.Application.Abstractions.Identity;
using Messenger.Infrastructure.Services.Options;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Services
{
    public sealed class TokenHashService : ITokenHashService
    {
        private readonly HashSettings _hashSettings;

        public TokenHashService(IOptions<HashSettings> hashSettings)
        {
            _hashSettings = hashSettings.Value;
        }

        public string Hash(string token)
        {
            var hash = BCrypt.Net.BCrypt.EnhancedHashPassword(
                token, _hashSettings.WorkFactor.Value);

            return hash;
        }

        public bool Verify(string token, string? hash)
        {
            if (hash is null)
            {
                return false;
            }

            var result = BCrypt.Net.BCrypt.EnhancedVerify(
                token, hash);

            return result;
        }
    }
}
