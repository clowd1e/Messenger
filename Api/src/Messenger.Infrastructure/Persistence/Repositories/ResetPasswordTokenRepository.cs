using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Messenger.Domain.Aggregates.ResetPasswordTokens.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public sealed class ResetPasswordTokenRepository
        : IResetPasswordTokenRepository
    {
        private readonly MessengerDbContext _context;

        public ResetPasswordTokenRepository(
            MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountActiveTokensAsync(
            UserId id,
            CancellationToken cancellationToken)
        {
            var activeTokens = await _context.ResetPasswordTokens
                .Include(token => token.User)
                .AsNoTracking()
                .Where(token => token.User.Id == id)
                .ToListAsync(cancellationToken);

            var activeTokensCount = activeTokens
                .Count(token => token.IsActive);

            return activeTokensCount;
        }

        public Task DeleteAsync(
            ResetPasswordToken resetPasswordToken)
        {
            _context.ResetPasswordTokens.Remove(resetPasswordToken);

            return Task.CompletedTask;
        }

        public async Task<ResetPasswordToken?> GetByIdWithUserAsync(
            ResetPasswordTokenId resetPasswordTokenId,
            CancellationToken cancellationToken = default)
        {
            return await _context.ResetPasswordTokens
                .Include(token => token.User)
                .FirstOrDefaultAsync(
                    token => token.Id == resetPasswordTokenId,
                    cancellationToken);
        }

        public async Task InsertAsync(
            ResetPasswordToken resetPasswordToken,
            CancellationToken cancellationToken = default)
        {
            await _context.ResetPasswordTokens
                .AddAsync(resetPasswordToken, cancellationToken);
        }
    }
}
