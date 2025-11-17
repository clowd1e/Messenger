using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Domain.Aggregates.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<User?> GetByIdWithPrivateChatsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<User?> GetByUsernameAsync(
            Username username,
            CancellationToken cancellationToken = default);

        Task<User?> GetByEmailAsync(
            Email email,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetAllExceptCurrentAsync(
            UserId currentUserId,
            CancellationToken cancellationToken = default);

        Task<User[]> GetAllByIdsAsync(
            IEnumerable<UserId> userIds,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Username username,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Email email,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Username username,
            Email email,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> SearchUsersByNameOrUsernameAsync(
            string searchTerm,
            UserId currentUserId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetUsersWithUnconfirmedEmailsAsync(
            DateTime registrationCutoffDate,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(
            IEnumerable<User> usersWithUnconfirmedEmails);
    }
}
