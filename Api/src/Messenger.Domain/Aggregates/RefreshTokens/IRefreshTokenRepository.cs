using Messenger.Domain.Aggregates.RefreshTokens.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Domain.Aggregates.RefreshTokens
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetBySessionIdWithUserAsync(
            SessionId sessionId,
            UserId userId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken = default);

        Task<int> GetUserSessionsCountAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<RefreshToken>> GetExpiredRefreshTokensAsync(
            CancellationToken cancellationToken = default);

        Task RemoveAsync(RefreshToken refreshToken);

        Task RemoveAsync(IEnumerable<RefreshToken> refreshTokens);
    }
}
