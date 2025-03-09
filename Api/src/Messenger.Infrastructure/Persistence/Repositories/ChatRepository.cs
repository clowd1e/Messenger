using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Messenger.Infrastructure.Persistence.Repositories
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

        public async Task<bool> ExistsAsync(
            UserId inviterId,
            UserId inviteeId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats.AnyAsync(
                c => c.Users.Any(u => u.Id == inviterId) &&
                     c.Users.Any(u => u.Id == inviteeId), cancellationToken);
        }

        public async Task<IEnumerable<Chat>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats.ToListAsync(cancellationToken);
        }

        public async Task<Chat?> GetByIdWithUsersAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats
                .Include(chat => chat.Users)
                .FirstOrDefaultAsync(chat => chat.Id == chatId, cancellationToken);
        }

        public async Task<IEnumerable<Chat>> GetUserChatsWithLastMessage(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats
                .Where(chat => chat.Users.Any(user => user.Id == userId))
                .Include(chat => chat.Users)
                .Include(IncludeLastMessage())
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Chat>> GetUserChatsPaginatedWithLastMessage(
            UserId userId,
            int page,
            int pageSize,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default)
        {
            var chats = await _context.Chats
                .Where(chat => chat.Users.Any(user => user.Id == userId))
                .Include(chat => chat.Users)
                .Include(IncludeLastMessage())
                .ToListAsync(cancellationToken);

            var result = chats
                .Where(chat => chat.CreationDate.Value <= retrievalCutoff)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return result;
        }

        public async Task<int> CountUserChatsAsync(
            UserId userId,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default)
        {
            var chats = await _context.Chats
                .Where(chat => chat.Users.Any(user => user.Id == userId))
                .ToListAsync(cancellationToken);

            var result = chats
                .Count(chat => chat.CreationDate.Value <= retrievalCutoff);

            return result;
        }

        public async Task<bool> IsUserInChatAsync(
            UserId userId,
            ChatId chatId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats
                .AnyAsync(chat => chat.Id == chatId &&
                    chat.Users.Any(user => user.Id == userId),
                cancellationToken);
        }

        public async Task InsertAsync(
            Chat chat,
            CancellationToken cancellationToken = default)
        {
            await _context.Chats.AddAsync(chat, cancellationToken);
        }

        private static Expression<Func<Chat, IEnumerable<Message>>> IncludeLastMessage()
        {
            return chat => chat.Messages
                .OrderByDescending(message => message.Timestamp)
                .Take(1);
        }
    }
}
