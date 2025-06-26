using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Users.Queries.GetAllExceptCurrent
{
    public sealed record GetAllUsersExceptCurrentQuery()
        : IQuery<IEnumerable<ShortUserResponse>>;
}
