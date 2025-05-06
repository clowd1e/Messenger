using Messenger.Application.Abstractions.Emails;
using Messenger.Infrastructure.Services.Emails.Options;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Services.Emails
{
    internal sealed class EmailLinkGenerator : IEmailLinkGenerator
    {
        private readonly EmailLinkGeneratorSettings _settings;

        public EmailLinkGenerator(
            IOptions<EmailLinkGeneratorSettings> emailLinkGeneratorSettings)
        {
            _settings = emailLinkGeneratorSettings.Value;
        }

        public string GenerateConfirmationEmailLink(string userId, string tokenId, string token)
        {
            var confirmationLink = @$"{_settings.ClientAppUrl}/{_settings.EmailConfirmationPath}?userId={userId}&tokenId={tokenId}&token={token}";

            return confirmationLink;
        }

        public string GeneratePasswordRecoveryEmailLink(string userId, string tokenId, string token)
        {
            var passwordRecoveryLink = @$"{_settings.ClientAppUrl}/{_settings.PasswordRecoveryPath}?userId={userId}&tokenId={tokenId}&token={token}";

            return passwordRecoveryLink;
        }
    }
}
