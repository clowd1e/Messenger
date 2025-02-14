using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task<IEnumerable<Chat>> GetUserChatsPaginated(
            UserId userId,
            int page,
            int pageSize,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default)
        {
            var chats = await _context.Chats
                .Where(chat => chat.Users.Any(user => user.Id == userId))
                .Include(chat => chat.Users)
                .ToListAsync(cancellationToken);

            var result = chats
                .Where(chat => chat.CreationDate.Value <= retrievalCutoff)
                .OrderByDescending(GetLatestMessageTimestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return result;
        }

        public async Task<IEnumerable<Message>> GetChatMessagesPaginated(
            ChatId chatId,
            int page,
            int pageSize,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(
                chat => chat.Id == chatId,
                cancellationToken);

            var result = chat.Messages
                .Where(message => message.Timestamp.Value <= retrievalCutoff)
                .OrderByDescending(message => message.Timestamp.Value)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Reverse();

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

        public async Task<int> CountChatMessagesAsync(
            ChatId chatId,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default)
        {
            var chat = await _context.Chats
                .FirstOrDefaultAsync(chat => chat.Id == chatId, cancellationToken);

            var result = chat.Messages
                .Count(message => message.Timestamp.Value <= retrievalCutoff);

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

        private static DateTime GetLatestMessageTimestamp(Chat chat)
        {
            return chat.Messages.OrderByDescending(
                    message => message.Timestamp.Value)
                .First().Timestamp.Value;
        }
    }
}
