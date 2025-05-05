using Messenger.Application.Abstractions.Emails;
using Razor.Templating.Core;

namespace Messenger.Infrastructure.Services.Emails
{
    internal sealed class EmailLetterGenerator : IEmailLetterGenerator
    {
        public async Task<Letter> GenerateConfirmationLetter(string confirmationLink)
        {
            var emailBody = await RazorTemplateEngine.RenderAsync(
                "EmailConfirmationTemplate",
                confirmationLink);

            return new Letter("Email Confirmation", emailBody);
        }

        public async Task<Letter> GeneratePasswordRecoveryLetter(string passwordRecoveryLink)
        {
            var emailBody = await RazorTemplateEngine.RenderAsync(
                "PasswordRecoveryTemplate",
                passwordRecoveryLink);

            return new Letter("Password Recovery", emailBody);
        }
    }
}
