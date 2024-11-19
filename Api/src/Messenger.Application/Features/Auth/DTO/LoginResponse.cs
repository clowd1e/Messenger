namespace Messenger.Application.Features.Auth.DTO
{
    public sealed record LoginResponse(
        string AccessToken,
        string RefreshToken);
}
