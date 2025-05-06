using Messenger.Domain.Aggregates.ConfirmEmailTokens.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Domain.Aggregates.ConfirmEmailTokens
{
    public interface IConfirmEmailTokenRepository
    {
        Task<ConfirmEmailToken?> GetByIdWithUserAsync(
            ConfirmEmailTokenId confirmEmailTokenId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            ConfirmEmailToken confirmEmailToken,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            ConfirmEmailToken confirmEmailToken);

        Task<int> CountActiveTokensAsync(
            UserId id,
            CancellationToken cancellationToken);
    }
}
