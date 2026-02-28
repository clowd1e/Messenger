using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Messages.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Domain.Aggregates.Messages
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(
            MessageId messageId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Message>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Message message,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Message>> GetChatMessagesPaginated(
            UserId requestingUserId,
            ChatId chatId,
            int page,
            int pageSize,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default);

        Task<int> CountChatMessagesAsync(
            UserId requestingUserId,
            ChatId chatId,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Message message);

        Task DeleteAsync(Message message);
    }
}
