namespace Messenger.Application.Abstractions.Emails
{
    public interface IEmailLinkGenerator
    {
        string GenerateConfirmationEmailLink(
            string userId,
            string token);
    }
}
