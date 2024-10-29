using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Domain.Aggregates.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<User?> GetByUsername(
            Username username,
            CancellationToken cancellationToken = default);

        Task<User?> GetByEmail(
            Email email,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetAllAsync(
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
    }
}
