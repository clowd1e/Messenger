using Messenger.Domain.Aggregates.Messages.ValueObjects;

namespace Messenger.Domain.Aggregates.Messages
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(
            MessageId messageId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Message>> GetAllAsync(
            int section,
            int sectionSize,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            MessageId messageId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Message message,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            Message message);

        Task DeleteAsync(
            Message message);
    }
}
