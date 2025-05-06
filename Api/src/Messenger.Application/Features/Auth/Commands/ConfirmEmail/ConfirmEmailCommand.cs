using MediatR;

namespace Messenger.Application.Features.Auth.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(
        Guid UserId,
        Guid TokenId,
        string Token) : IRequest<Result>;
}
