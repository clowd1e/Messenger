﻿using Messenger.Application.Identity;

namespace Messenger.Application.Abstractions.Emails
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(
            string recipientEmail,
            string userId,
            ApplicationUser identityUser);
    }
}
