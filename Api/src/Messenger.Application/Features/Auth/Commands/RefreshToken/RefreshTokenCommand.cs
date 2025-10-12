using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Auth.DTO.Response;

namespace Messenger.Application.Features.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string AccessToken,
        string RefreshToken,
        string SessionId) : ICommand<RefreshTokenResponse>;
}
