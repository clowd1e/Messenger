using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.RefreshTokens;
using Messenger.Domain.Aggregates.RefreshTokens.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly MessengerDbContext _context;

        public RefreshTokenRepository(
            MessengerDbContext context)
        {
            _context = context;
        }

        public Task DeleteAsync(
            RefreshToken refreshToken)
        {
            _context.Remove(refreshToken);

            return Task.CompletedTask;
        }

        public async Task<RefreshToken?> GetBySessionIdWithUserAsync(
            SessionId sessionId,
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .Include(token => token.User)
                .FirstOrDefaultAsync(
                    token => token.SessionId == sessionId &&
                        token.UserId == userId, cancellationToken);
        }

        public async Task<IEnumerable<RefreshToken>> GetExpiredRefreshTokensAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .Where(token => token.ExpiresAt <= Timestamp.UtcNow())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetUserSessionsCountAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .Include(token => token.User)
                .AsNoTracking()
                .CountAsync(token => token.User.Id == userId, cancellationToken);
        }

        public async Task InsertAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken = default)
        {
            await _context.RefreshTokens
                .AddAsync(refreshToken, cancellationToken);
        }
    }
}
