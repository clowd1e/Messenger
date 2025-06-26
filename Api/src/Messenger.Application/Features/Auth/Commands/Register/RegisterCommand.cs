using Messenger.Application.Abstractions.Messaging;

namespace Messenger.Application.Features.Auth.Commands.Register
{
    public sealed record RegisterCommand(
        string Username,
        string Name,
        string Email,
        string Password) : ICommand;
}
