using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistense.Repositories
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
            return await _context.Users.ToListAsync();
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
            return await _context.Users.FindAsync(userId, cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(
            Username username,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(
                x => x.Username == username,
                cancellationToken);
        }

        public async Task InsertAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }
    }
}
