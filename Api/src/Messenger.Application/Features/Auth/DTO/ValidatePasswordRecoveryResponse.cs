namespace Messenger.Application.Features.Auth.DTO
{
    public sealed record ValidatePasswordRecoveryResponse(
        DateTime ExpiresAt);
}
