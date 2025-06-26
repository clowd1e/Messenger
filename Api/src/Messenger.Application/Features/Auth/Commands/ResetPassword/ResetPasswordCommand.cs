using Messenger.Application.Abstractions.Messaging;

namespace Messenger.Application.Features.Auth.Commands.ResetPassword
{
    public sealed record ResetPasswordCommand(
        Guid UserId,
        Guid TokenId,
        string Token,
        string NewPassword) : ICommand;
}
