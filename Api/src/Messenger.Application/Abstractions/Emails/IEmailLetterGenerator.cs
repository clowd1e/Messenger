namespace Messenger.Application.Abstractions.Emails
{
    public interface IEmailLetterGenerator
    {
        Task<Letter> GenerateConfirmationLetter(string confirmationLink);

        Task<Letter> GeneratePasswordRecoveryLetter(string passwordRecoveryLink);
    }
}
