namespace Messenger.Application.Features.Auth.DTO.Response
{
    public sealed record ValidateEmailConfirmationResponse(
        DateTime ExpiresAt);
}
