using FluentEmail.Core;
using Messenger.Application.Abstractions.Emails;

namespace Messenger.Infrastructure.Services.Emails
{
    internal sealed class EmailSender : IEmailSender
    {
        private readonly IFluentEmail _fluentEmail;
        
        public EmailSender(IFluentEmail email)
        {
            _fluentEmail = email;
        }

        public async Task SendEmailAsync(string recipientEmail, Letter letter)
        {
            await _fluentEmail
                .To(recipientEmail)
                .Subject(letter.Subject)
                .Body(letter.Body, isHtml: true)
                .SendAsync();
        }
    }
}
