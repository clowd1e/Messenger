using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Users.Queries.GetAll
{
    public sealed record GetAllUsersQuery()
        : IQuery<IEnumerable<UserResponse>>;
}
