using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Auth.DTO.Response;

namespace Messenger.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand(
        string Email,
        string Password,
        string SessionId) : ICommand<LoginResponse>;
}
