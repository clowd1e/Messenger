using Messenger.Application.Abstractions.Emails;
using Messenger.Application.Identity;
using Microsoft.AspNetCore.Identity;

namespace Messenger.Infrastructure.Services.Emails
{
    internal sealed class EmailTokenGenerator : IEmailTokenGenerator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailTokenGenerator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GenerateEmailConfirmationToken(
            ApplicationUser identityUser)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
        }
    }
}
