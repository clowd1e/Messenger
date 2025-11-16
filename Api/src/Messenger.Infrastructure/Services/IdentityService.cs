using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Identity;
using Messenger.Application.Identity.Options;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Services
{
    public sealed class IdentityService : IIdentityService<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LoginSettings _loginSettings;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<LoginSettings> loginSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _loginSettings = loginSettings.Value;
        }

        public Result ConfirmEmail(
            ApplicationUser identityUser)
        {
            ArgumentNullException.ThrowIfNull(identityUser);

            identityUser.EmailConfirmed = true;

            return Result.Success();
        }

        public async Task CreateAsync(
            ApplicationUser identityUser,
            string password)
        {
            ArgumentNullException.ThrowIfNull(identityUser);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            var result = await _userManager.CreateAsync(identityUser, password);

            HandleIdentityResult(result, "Failed to create identity user.");
        }

        public async Task DeleteAsync(ApplicationUser identityUser)
        {
            ArgumentNullException.ThrowIfNull(identityUser);

            var result = await _userManager.DeleteAsync(identityUser);

            HandleIdentityResult(result, "Failed to delete identity user.");
        }

        public async Task<ApplicationUser?> GetByEmailAsync(Email email)
        {
            ArgumentNullException.ThrowIfNull(email);

            var identityUser = await _userManager.FindByEmailAsync(email.Value);

            return identityUser;
        }

        public async Task<ApplicationUser?> GetByIdAsync(
            UserId userId)
        {
            var identityUser = await _userManager.FindByIdAsync(userId.Value.ToString());

            return identityUser;
        }

        public async Task<Result> LoginAsync(
            ApplicationUser identityUser,
            string password)
        {
            ArgumentNullException.ThrowIfNull(identityUser);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            var result = await _signInManager.PasswordSignInAsync(
                identityUser,
                password,
                isPersistent: _loginSettings.IsPersistent,
                lockoutOnFailure: _loginSettings.LockoutOnFailure);

            if (!result.Succeeded)
            {
                return UserErrors.InvalidCredentials;
            }

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(
            ApplicationUser identityUser,
            string newPassword)
        {
            ArgumentNullException.ThrowIfNull(identityUser);
            ArgumentException.ThrowIfNullOrWhiteSpace(newPassword);

            var result = await _userManager.RemovePasswordAsync(identityUser);

            HandleIdentityResult(result, "Failed to remove password.");

            var addPasswordResult = await _userManager.AddPasswordAsync(
                identityUser,
                newPassword);

            HandleIdentityResult(addPasswordResult, "Failed to add password.");

            return Result.Success();
        }

        private static void HandleIdentityResult(IdentityResult result, string errorMessage)
        {
            if (result.Succeeded)
            {
                return;
            }

            var errors = result.Errors.Select(x => x.Description);
            var errorsString = string.Join(Environment.NewLine, errors);

            throw new InvalidOperationException($"{errorMessage}\r\nErrors: {errorsString}");
        }
    }
}
