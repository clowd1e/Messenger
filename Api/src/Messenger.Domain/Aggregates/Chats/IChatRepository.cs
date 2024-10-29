using Messenger.Domain.Aggregates.Chats.ValueObjects;

namespace Messenger.Domain.Aggregates.Chats
{
    public interface IChatRepository
    {
        Task<Chat?> GetByIdAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Chat>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Chat chat,
            CancellationToken cancellationToken = default);
    }
}
