using Messenger.Application.Abstractions.Messaging;

namespace Messenger.Application.Features.Auth.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(
        Guid UserId,
        Guid TokenId,
        string Token) : ICommand;
}
