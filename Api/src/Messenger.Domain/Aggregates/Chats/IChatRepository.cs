using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Domain.Aggregates.Chats
{
    public interface IChatRepository
    {
        Task<Chat?> GetByIdAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Chat>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Chat>> GetUserChats(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            UserId inviterId,
            UserId inviteeId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Chat chat,
            CancellationToken cancellationToken = default);
    }
}
