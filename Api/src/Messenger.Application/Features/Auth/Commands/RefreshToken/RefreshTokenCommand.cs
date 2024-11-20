using MediatR;
using Messenger.Application.Features.Auth.DTO;

namespace Messenger.Application.Features.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string? AccessToken,
        string? RefreshToken) : IRequest<Result<RefreshTokenResponse>>;
}
