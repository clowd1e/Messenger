using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Users.Mappers
{
    internal sealed class UserToSearchUserResponseMapper
        : Mapper<User, SearchUserResponse>
    {
        public override SearchUserResponse Map(User source)
        {
            return new SearchUserResponse(
                source.Id.Value,
                source.Name.Value,
                source.Username.Value,
                source.IconUri?.Value);
        }
    }
}
