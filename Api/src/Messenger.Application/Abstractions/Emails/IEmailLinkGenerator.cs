namespace Messenger.Application.Abstractions.Emails
{
    public interface IEmailLinkGenerator
    {
        string GenerateConfirmationEmailLink(
            string userId,
            string tokenId,
            string token);

        string GeneratePasswordRecoveryEmailLink(
            string userId,
            string tokenId,
            string token);
    }
}
