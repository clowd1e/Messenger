using Messenger.Application.Abstractions.Emails;
using Messenger.Application.Identity;

namespace Messenger.Application.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailLinkGenerator _emailLinkGenerator;
        private readonly IEmailLetterGenerator _emailLetterGenerator;

        public EmailService(
            IEmailSender emailSender,
            IEmailLinkGenerator emailLinkGenerator,
            IEmailLetterGenerator emailLetterGenerator)
        {
            _emailSender = emailSender;
            _emailLinkGenerator = emailLinkGenerator;
            _emailLetterGenerator = emailLetterGenerator;
        }

        public async Task SendConfirmationEmailAsync(
            string recipientEmail,
            string userId,
            string tokenId,
            string token)
        {
            var escapedToken = Uri.EscapeDataString(token);

            var confirmationLink = _emailLinkGenerator
                .GenerateConfirmationEmailLink(userId, tokenId, escapedToken);

            var letter = await _emailLetterGenerator.GenerateConfirmationLetter(
                confirmationLink);

            await _emailSender.SendEmailAsync(
                recipientEmail,
                letter);
        }

        public async Task SendPasswordRecoveryEmailAsync(
            string recipientEmail,
            string userId,
            string tokenId,
            string token)
        {
            var escapedToken = Uri.EscapeDataString(token);

            var passwordRecoveryLink = _emailLinkGenerator
                .GeneratePasswordRecoveryEmailLink(userId, tokenId, escapedToken);

            var letter = await _emailLetterGenerator.GeneratePasswordRecoveryLetter(
                passwordRecoveryLink);

            await _emailSender.SendEmailAsync(
                recipientEmail,
                letter);
        }
    }
}
