using Messenger.Domain.Aggregates.ResetPasswordTokens.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Domain.Aggregates.ResetPasswordTokens
{
    public interface IResetPasswordTokenRepository
    {
        Task<ResetPasswordToken?> GetByIdWithUserAsync(
            ResetPasswordTokenId resetPasswordTokenId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            ResetPasswordToken resetPasswordToken,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            ResetPasswordToken resetPasswordToken);

        Task<int> CountActiveTokensAsync(
            UserId id,
            CancellationToken cancellationToken);
    }
}
