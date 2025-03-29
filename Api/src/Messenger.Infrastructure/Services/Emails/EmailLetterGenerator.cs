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
    }
}
