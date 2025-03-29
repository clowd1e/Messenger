using Messenger.Application.Abstractions.Emails;
using Messenger.Application.Identity;

namespace Messenger.Application.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailLinkGenerator _emailLinkGenerator;
        private readonly IEmailLetterGenerator _emailLetterGenerator;
        private readonly IEmailTokenGenerator _emailTokenGenerator;

        public EmailService(
            IEmailSender emailSender,
            IEmailLinkGenerator emailLinkGenerator,
            IEmailLetterGenerator emailLetterGenerator,
            IEmailTokenGenerator emailTokenGenerator)
        {
            _emailSender = emailSender;
            _emailLinkGenerator = emailLinkGenerator;
            _emailLetterGenerator = emailLetterGenerator;
            _emailTokenGenerator = emailTokenGenerator;
        }

        public async Task SendConfirmationEmailAsync(
            string recipientEmail,
            string userId,
            ApplicationUser identityUser)
        {
            var token = await _emailTokenGenerator
                .GenerateEmailConfirmationToken(identityUser);

            var escapedToken = Uri.EscapeDataString(token);

            var confirmationLink = _emailLinkGenerator
                .GenerateConfirmationEmailLink(userId, escapedToken);

            var letter = await _emailLetterGenerator.GenerateConfirmationLetter(
                confirmationLink);

            await _emailSender.SendEmailAsync(
                recipientEmail,
                letter);
        }
    }
}
