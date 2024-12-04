using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistense.Repositories
{
    public sealed class ChatRepository : IChatRepository
    {
        private readonly MessengerDbContext _context;

        public ChatRepository(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats.AnyAsync(
                x => x.Id == chatId, cancellationToken);
        }

        public async Task<IEnumerable<Chat>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats.ToListAsync(cancellationToken);
        }

        public async Task<Chat?> GetByIdAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats
                .Include(chat => chat.Users)
                .FirstOrDefaultAsync(chat => chat.Id == chatId, cancellationToken);
        }

        public async Task<IEnumerable<Chat>> GetUserChats(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats
                .Where(chat => chat.Users.Any(user => user.Id == userId))
                .Include(chat => chat.Users)
                .ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(
            Chat chat,
            CancellationToken cancellationToken = default)
        {
            await _context.Chats.AddAsync(chat, cancellationToken);
        }
    }
}
