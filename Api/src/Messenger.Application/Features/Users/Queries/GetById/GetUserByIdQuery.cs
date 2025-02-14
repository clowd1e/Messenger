using MediatR;
using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Users.Queries.GetById
{
    public sealed record GetUserByIdQuery(
        Guid UserId) : IRequest<Result<UserResponse>>;
}
