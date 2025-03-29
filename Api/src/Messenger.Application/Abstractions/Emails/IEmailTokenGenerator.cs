using Messenger.Application.Identity;

namespace Messenger.Application.Abstractions.Emails
{
    public interface IEmailTokenGenerator
    {
        Task<string> GenerateEmailConfirmationToken(ApplicationUser identityUser);
    }
}
