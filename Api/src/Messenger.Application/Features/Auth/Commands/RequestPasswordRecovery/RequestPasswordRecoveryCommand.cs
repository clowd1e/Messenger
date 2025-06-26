using Messenger.Application.Abstractions.Messaging;

namespace Messenger.Application.Features.Auth.Commands.RequestPasswordRecovery
{
    public sealed record RequestPasswordRecoveryCommand(
        string Email) : ICommand;
}
