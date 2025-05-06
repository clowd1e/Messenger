using Messenger.Domain.Aggregates.ConfirmEmailTokens;
using Messenger.Domain.Aggregates.ConfirmEmailTokens.ValueObjects;
using Messenger.Domain.Aggregates.ResetPasswordTokens.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public sealed class ConfirmEmailTokenRepository
        : IConfirmEmailTokenRepository
    {
        private readonly MessengerDbContext _context;

        public ConfirmEmailTokenRepository(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountActiveTokensAsync(
            UserId id,
            CancellationToken cancellationToken)
        {
            var activeTokens = await _context.ConfirmEmailTokens
                .Include(token => token.User)
                .AsNoTracking()
                .Where(token => token.User.Id == id)
                .ToListAsync(cancellationToken);

            var activeTokensCount = activeTokens
                .Count(token => token.IsActive);

            return activeTokensCount;
        }

        public Task DeleteAsync(
            ConfirmEmailToken confirmEmailToken)
        {
            _context.ConfirmEmailTokens.Remove(confirmEmailToken);

            return Task.CompletedTask;
        }

        public async Task<ConfirmEmailToken?> GetByIdWithUserAsync(
            ConfirmEmailTokenId confirmEmailTokenId,
            CancellationToken cancellationToken = default)
        {
            return await _context.ConfirmEmailTokens
                .Include(token => token.User)
                .FirstOrDefaultAsync(
                    token => token.Id == confirmEmailTokenId,
                    cancellationToken);
        }

        public async Task InsertAsync(
            ConfirmEmailToken confirmEmailToken,
            CancellationToken cancellationToken = default)
        {
            await _context.ConfirmEmailTokens
                .AddAsync(confirmEmailToken, cancellationToken);
        }
    }
}
