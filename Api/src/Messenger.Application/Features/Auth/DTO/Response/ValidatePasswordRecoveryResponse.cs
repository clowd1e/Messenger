namespace Messenger.Application.Features.Auth.DTO.Response
{
    public sealed record ValidatePasswordRecoveryResponse(
        DateTime ExpiresAt);
}
