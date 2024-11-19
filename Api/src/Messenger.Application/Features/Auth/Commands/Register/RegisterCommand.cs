using MediatR;

namespace Messenger.Application.Features.Auth.Commands.Register
{
    public sealed record RegisterCommand(
        string? Username,
        string? Email,
        string? Password) : IRequest<Result>;
}
