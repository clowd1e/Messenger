using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Domain.Aggregates.Chats
{
    public interface IChatRepository
    {
        Task<Chat?> GetByIdWithUsersAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Chat>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Chat>> GetUserChatsWithLastMessage(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Chat>> GetUserChatsPaginatedWithLastMessage(
            UserId userId,
            int page,
            int pageSize,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default);

        Task<int> CountUserChatsAsync(
            UserId userId,
            DateTime retrievalCutoff,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            UserId inviterId,
            UserId inviteeId,
            CancellationToken cancellationToken = default);

        Task<bool> IsUserInChatAsync(
            UserId userId,
            ChatId chatId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Chat chat,
            CancellationToken cancellationToken = default);
    }
}
