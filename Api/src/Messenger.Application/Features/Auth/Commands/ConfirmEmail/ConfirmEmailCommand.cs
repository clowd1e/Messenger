using MediatR;

namespace Messenger.Application.Features.Auth.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(
        Guid UserId,
        string Token) : IRequest<Result>;
}
