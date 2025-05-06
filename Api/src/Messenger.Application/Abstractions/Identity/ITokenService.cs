using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Abstractions.Identity
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        string GenerateResetPasswordToken();

        string GenerateEmailConfirmationToken();

        Task<Result> ValidateAccessTokenAsync(
            string token, bool validateLifetime = true);
    }
}
