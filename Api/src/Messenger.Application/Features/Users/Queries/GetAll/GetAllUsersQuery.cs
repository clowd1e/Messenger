using MediatR;
using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Users.Queries.GetAll
{
    public sealed record GetAllUsersQuery() 
        : IRequest<Result<IEnumerable<UserResponse>>>;
}
