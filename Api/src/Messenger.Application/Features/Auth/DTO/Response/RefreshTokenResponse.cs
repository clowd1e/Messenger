namespace Messenger.Application.Features.Auth.DTO.Response
{
    public sealed record RefreshTokenResponse(
        string AccessToken,
        string RefreshToken);
}
