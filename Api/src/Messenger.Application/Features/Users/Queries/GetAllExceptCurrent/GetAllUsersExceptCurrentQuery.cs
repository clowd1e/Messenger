using MediatR;
using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Users.Queries.GetAllExceptCurrent
{
    public sealed record GetAllUsersExceptCurrentQuery()
        : IRequest<Result<IEnumerable<ShortUserResponse>>>;
}
