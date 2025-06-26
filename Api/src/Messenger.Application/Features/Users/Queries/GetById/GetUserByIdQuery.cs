using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Users.Queries.GetById
{
    public sealed record GetUserByIdQuery(
        Guid UserId) : IQuery<UserResponse>;
}
