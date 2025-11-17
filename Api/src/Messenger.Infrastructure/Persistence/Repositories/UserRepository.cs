using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly MessengerDbContext _context;

        public UserRepository(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(
            UserId userId, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(x => x.Id == userId);
        }

        public async Task<bool> ExistsAsync(
            Username username, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(
                x => x.Username == username,
                cancellationToken);
        }

        public async Task<bool> ExistsAsync(
            Email email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(
                x => x.Email == email,
                cancellationToken);
        }

        public Task<bool> ExistsAsync(
            Username username,
            Email email,
            CancellationToken cancellationToken = default)
        {
            return _context.Users.AnyAsync(
                x => x.Username == username || x.Email == email,
                cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Users.ToListAsync(cancellationToken);
        }

        public async Task<User[]> GetAllByIdsAsync(
            IEnumerable<UserId> userIds,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllExceptCurrentAsync(
            UserId currentUserId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id != currentUserId)
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(
            Email email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);
        }

        public async Task<User?> GetByIdAsync(
            UserId userId, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User?> GetByIdWithPrivateChatsAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(u => u.Chats)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(
            Username username,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(
                x => x.Username == username,
                cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersWithUnconfirmedEmailsAsync(
            DateTime registrationCutoffDate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => !u.EmailConfirmed && ((DateTime)u.RegistrationDate) <= registrationCutoffDate)
                .ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public Task RemoveAsync(
            IEnumerable<User> usersWithUnconfirmedEmails)
        {
            _context.Users.RemoveRange(usersWithUnconfirmedEmails);

            return Task.CompletedTask;
        }

        // TODO: refactor search to use full-text search for better performance
        // and apply algorithms like fuzzy search or phonetic matching
        public async Task<IEnumerable<User>> SearchUsersByNameOrUsernameAsync(
            string searchTerm,
            UserId currentUserId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.Id != currentUserId && 
                        u.EmailConfirmed &&
                        (
                            ((string)u.Name).ToLower().Contains(searchTerm) ||
                            ((string)u.Username).ToLower().Contains(searchTerm)
                        ))
                .ToListAsync(cancellationToken);
        }
    }
}
