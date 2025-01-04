using MediatR;

namespace Messenger.Application.Features.Users.Commands.RemoveIcon
{
    public sealed record RemoveUserIconCommand() : IRequest<Result>;
}
