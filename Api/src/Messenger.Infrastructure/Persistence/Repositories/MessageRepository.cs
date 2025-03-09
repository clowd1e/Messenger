using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Messages.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public sealed class MessageRepository : IMessageRepository
    {
        private readonly MessengerDbContext _context;

        public MessageRepository(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(
            MessageId messageId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Messages.AnyAsync(
                x => x.Id == messageId, cancellationToken);
        }

        public async Task<IEnumerable<Message>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Messages
                .Include(message => message.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<Message?> GetByIdAsync(
            MessageId messageId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Messages
                .Include(message => message.User)
                .FirstOrDefaultAsync(message => message.Id == messageId, cancellationToken);
        }

        public async Task<IEnumerable<Message>> GetChatMessagesPaginated(
            ChatId chatId,
            int page,
            int pageSize,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default)
        {
            var messages = await _context.Messages
                .Where(message => message.Chat.Id == chatId)
                .Include(message => message.User)
                .ToListAsync(cancellationToken);

            var result = messages
                .Where(message => message.Timestamp.Value <= retrievalCutoff)
                .OrderByDescending(message => message.Timestamp.Value)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Reverse();

            return result;
        }

        public async Task<int> CountChatMessagesAsync(
            ChatId chatId,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default)
        {
            var messages = await _context.Messages
                .Where(message => message.Chat.Id == chatId)
                .ToListAsync(cancellationToken);

            var result = messages
                .Count(message => message.Timestamp.Value <= retrievalCutoff);

            return result;
        }
        public async Task InsertAsync(
            Message message,
            CancellationToken cancellationToken = default)
        {
            await _context.Messages.AddAsync(message, cancellationToken);
        }

        public Task UpdateAsync(Message message)
        {
            _context.Messages.Update(message);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Message message)
        {
            _context.Messages.Remove(message);

            return Task.CompletedTask;
        }
    }
}
