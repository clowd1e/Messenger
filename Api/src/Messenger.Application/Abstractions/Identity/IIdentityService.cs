using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Abstractions.Identity
{
    public interface IIdentityService<TIdentityUser>
        where TIdentityUser : class
    {
        Task CreateAsync(TIdentityUser identityUser, string password);

        Task DeleteAsync(TIdentityUser identityUser);

        Task DeleteAsync(IEnumerable<TIdentityUser> identityUsers);

        Task<TIdentityUser?> GetByEmailAsync(Email email);

        Task<TIdentityUser?> GetByIdAsync(UserId userId);

        Task<Result> LoginAsync(TIdentityUser identityUser, string password);

        Result ConfirmEmail(TIdentityUser identityUser);

        Task<Result> ResetPasswordAsync(
            TIdentityUser identityUser,
            string newPassword);

        Task<IEnumerable<TIdentityUser>> GetUsersByIds(IEnumerable<UserId> usersIds);
    }
}
