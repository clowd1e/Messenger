using Messenger.Application.Abstractions.Data;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Auth.Mappers
{
    internal sealed class UserToApplicationUserMapper
        : Mapper<User, ApplicationUser>
    {
        public override ApplicationUser Map(User source)
        {
            return new ApplicationUser
            {
                Id = source.Id.Value.ToString(),
                UserName = source.Username.Value,
                NormalizedUserName = source.Username.Value.ToUpper(),
                Email = source.Email.Value,
                NormalizedEmail = source.Email.Value.ToUpper()
            };
        }
    }
}
