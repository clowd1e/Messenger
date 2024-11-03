using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Users.Mappers
{
    public sealed class UserToUserResponseMapper
        : Mapper<User, UserResponse>
    {
        public override UserResponse Map(User source)
        {
            return new(
                Id: source.Id.Value,
                Username: source.Username.Value,
                Email: source.Email.Value);
        }
    }
}
