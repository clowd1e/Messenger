namespace Messenger.Application.Features.Users.DTO
{
    public sealed record UserResponse(
        Guid Id,
        string Username,
        string Name,
        string Email,
        string? IconUri);
}
