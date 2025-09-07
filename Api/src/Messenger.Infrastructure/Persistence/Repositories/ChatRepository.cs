using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Common.Timestamp;
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

        public async Task<bool> ChatExistsAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats.AnyAsync(
                chat => chat.Id == chatId, cancellationToken);
        }

        public async Task<bool> PrivateChatExistsAsync(
            UserId inviterId,
            UserId inviteeId,
            CancellationToken cancellationToken = default)
        {
            return await _context.PrivateChats.AnyAsync(
                pc => pc.Participants.Any(u => u.Id == inviterId) &&
                     pc.Participants.Any(u => u.Id == inviteeId), cancellationToken);
        }

        public async Task<IEnumerable<Chat>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats.ToListAsync(cancellationToken);
        }

        public async Task<Chat?> GetByIdWithUsersAndLastMessageAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default)
        {
            var chat = await _context.Chats
                .Include(chat => chat.Participants)
                .Include(IncludeLastMessage())
                .FirstOrDefaultAsync(chat => chat.Id == chatId, cancellationToken);

            if (chat is GroupChat groupChat)
            {
                await _context.Entry(groupChat)
                    .Collection(gc => gc.GroupMembers)
                    .LoadAsync(cancellationToken);
            }

            return chat;
        }

        public async Task<IEnumerable<Chat>> GetUserChatsWithLastMessage(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            var privateChats = await _context.PrivateChats
                .Where(chat => chat.Participants.Any(user => user.Id == userId))
                .Include(chat => chat.Participants)
                .Include(IncludeLastMessage())
                .ToListAsync(cancellationToken);

            var groupChats = await _context.GroupChats
                .Where(chat => chat.Participants.Any(participant => participant.Id == userId))
                .Include(chat => chat.Participants)
                .Include(chat => chat.GroupMembers)
                .Include(IncludeLastMessage())
                .ToListAsync(cancellationToken);

            var chats = privateChats
                .Concat(groupChats);

            return chats;
        }

        public async Task<IEnumerable<Chat>> GetUserChatsPaginatedWithLastMessage(
            UserId userId,
            int page,
            int pageSize,
            Timestamp retrievalCutoff,
            CancellationToken cancellationToken = default)
        {
            var chats = await _context.Chats
                // Filter chats where the user is a participant and the chat was created before or on the retrieval cutoff date
                .Where(chat => 
                    chat.Participants.Any(participant => participant.Id == userId) &&
                    chat.CreationDate <= retrievalCutoff)
                // Include participants for each chat
                .Include(chat => chat.Participants)
                // Include last message for each chat
                .Include(IncludeLastMessage())
                // Order by the timestamp of the last message in descending order
                .OrderByDescending(chat => chat.Messages.First().Timestamp)
                // Filter chats where the last message was sent before or on the retrieval cutoff date
                .Where(chat => chat.Messages.First().Timestamp <= retrievalCutoff)
                // Apply pagination
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            chats = chats.OrderByDescending(chat => chat.Messages.First().Timestamp.Value).ToList();

            var groupChatIds = chats.OfType<GroupChat>()
                .Select(gc => gc.Id)
                .ToList();

            // Load group members info for group chats
            if (groupChatIds.Count > 0)
            {
                await _context.GroupChats
                    .Where(gc => groupChatIds.Contains(gc.Id))
                    .Include(gc => gc.GroupMembers)
                    .LoadAsync(cancellationToken);
            }

            return chats;
        }

        public async Task<int> CountUserChatsAsync(
            UserId userId,
            Timestamp retrievalCutoff,
            CancellationToken cancellationToken = default)
        {
            var chatsCount = await _context.Chats
                .Where(chat => 
                    chat.Participants.Any(participant => participant.Id == userId) &&
                    chat.CreationDate <= retrievalCutoff)
                .CountAsync(cancellationToken);

            return chatsCount;
        }

        public async Task<bool> IsUserInChatAsync(
            UserId userId,
            ChatId chatId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Chats
                .AnyAsync(c => c.Id == chatId &&
                    c.Participants.Any(participant => participant.Id == userId),
                cancellationToken);
        }

        public async Task InsertPrivateChatAsync(
            PrivateChat privateChat,
            CancellationToken cancellationToken = default)
        {
            await _context.PrivateChats.AddAsync(privateChat, cancellationToken);
        }

        public async Task InsertGroupChatAsync(
            GroupChat groupChat,
            CancellationToken cancellationToken = default)
        {
            await _context.GroupChats.AddAsync(groupChat, cancellationToken);
        }

        private static Expression<Func<Chat, IEnumerable<Message>>> IncludeLastMessage()
        {
            return chat => chat.Messages
                .OrderByDescending(message => message.Timestamp)
                .Take(1);
        }
    }
}
