using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Shared;
using Messenger.Infrastructure.Authentication.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Services
{
    public sealed class IdentityService : IIdentityService<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LoginSettings _loginSettings;
        private readonly ITokenService _tokenService;
        private readonly ITokenHashService _tokenHashService;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<LoginSettings> loginSettings,
            ITokenService tokenService,
            ITokenHashService tokenHashService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _loginSettings = loginSettings.Value;
            _tokenService = tokenService;
            _tokenHashService = tokenHashService;
        }

        public async Task<Result> ConfirmEmailAsync(
            ApplicationUser identityUser,
            string token)
        {
            ArgumentNullException.ThrowIfNull(identityUser);
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

            var result = await _userManager.ConfirmEmailAsync(identityUser, token);

            if (!result.Succeeded)
            {
                return UserErrors.InvalidEmailConfirmationToken;
            }

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
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            var identityUser = await _userManager.FindByIdAsync(userId.Value.ToString());

            return identityUser;
        }

        public async Task<Result<ApplicationUser>> GetByRefreshTokenAsync(string refreshToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

            var users = await _userManager.Users.ToListAsync();

            var identityUser = users.FirstOrDefault(
                u => _tokenHashService.Verify(refreshToken, u.RefreshTokenHash));

            if (identityUser is null)
            {
                return Result.Failure<ApplicationUser>(UserErrors.InvalidRefreshToken);
            }

            return Result.Success(identityUser);
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

        public async Task PopulateRefreshTokenAsync(
            ApplicationUser identityUser,
            string refreshToken)
        {
            ArgumentNullException.ThrowIfNull(identityUser);
            ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

            var refreshTokenHash = _tokenHashService.Hash(refreshToken);

            identityUser.RefreshTokenHash = refreshTokenHash;

            identityUser.RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(
                _loginSettings.RefreshTokenExpiryInDays.Value);

            await _userManager.UpdateAsync(identityUser);
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

        public Result ValidateRefreshToken(ApplicationUser user)
        {
            if (user.RefreshTokenExpirationTime < DateTime.UtcNow)
            {
                return Result.Failure<ApplicationUser>(UserErrors.RefreshTokenExpired);
            }

            return Result.Success();
        }

        private void HandleIdentityResult(IdentityResult result, string errorMessage)
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
