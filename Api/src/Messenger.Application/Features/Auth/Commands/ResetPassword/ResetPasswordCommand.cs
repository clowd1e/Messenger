using MediatR;

namespace Messenger.Application.Features.Auth.Commands.ResetPassword
{
    public sealed record ResetPasswordCommand(
        Guid UserId,
        Guid TokenId,
        string Token,
        string NewPassword) : IRequest<Result>;
}
