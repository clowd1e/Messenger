using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Abstractions.Identity
{
    public interface IIdentityService<TIdentityUser>
        where TIdentityUser : class
    {
        Task CreateAsync(TIdentityUser identityUser, string password);

        Task DeleteAsync(TIdentityUser identityUser);

        Task<TIdentityUser?> GetByEmailAsync(Email email);

        Task<TIdentityUser?> GetByIdAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<Result> LoginAsync(TIdentityUser identityUser, string password);

        Task<Result> ConfirmEmailAsync(TIdentityUser identityUser);

        Task PopulateRefreshTokenAsync(TIdentityUser identityUser, string refreshToken);

        Result ValidateRefreshToken(TIdentityUser user);

        Task<Result<TIdentityUser>> GetByRefreshTokenAsync(string refreshToken);

        Task<Result> ResetPasswordAsync(
            TIdentityUser identityUser,
            string newPassword);
    }
}
