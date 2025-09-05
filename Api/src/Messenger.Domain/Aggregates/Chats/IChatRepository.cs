using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Domain.Aggregates.Chats
{
    public interface IChatRepository
    {
        Task<Chat?> GetByIdWithUsersAndLastMessageAsync(
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

        Task<bool> ChatExistsAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default);

        Task<bool> PrivateChatExistsAsync(
            UserId inviterId,
            UserId inviteeId,
            CancellationToken cancellationToken = default);

        Task<bool> IsUserInChatAsync(
            UserId userId,
            ChatId chatId,
            CancellationToken cancellationToken = default);

        Task InsertPrivateChatAsync(
            PrivateChat privateChat,
            CancellationToken cancellationToken = default);

        Task InsertGroupChatAsync(
            GroupChat groupChat,
            CancellationToken cancellationToken = default);
    }
}
