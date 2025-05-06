namespace Messenger.Application.Features.Auth.DTO.Response
{
    public sealed record LoginResponse(
        string AccessToken,
        string RefreshToken);
}
