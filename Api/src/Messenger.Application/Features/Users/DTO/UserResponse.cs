namespace Messenger.Application.Features.Users.DTO
{
    public sealed record UserResponse(
        Guid Id,
        string Username,
        string Email,
        string? IconUri);
}
