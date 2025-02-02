using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Users.Mappers
{
    internal sealed class UserToShortUserResponseMapper
        : Mapper<User, ShortUserResponse>
    {
        public override ShortUserResponse Map(User source)
        {
            return new(
                Id: source.Id.Value,
                Username: source.Username.Value,
                IconUri: source.IconUri?.Value);
        }
    }
}
