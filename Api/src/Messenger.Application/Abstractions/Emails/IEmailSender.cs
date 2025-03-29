namespace Messenger.Application.Abstractions.Emails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            string recipientEmail,
            Letter letter);
    }
}
