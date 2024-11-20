namespace Messenger.Application.Features.Auth.DTO
{
    public sealed record RefreshTokenResponse(
        string AccessToken,
        string RefreshToken);
}
