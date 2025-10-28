namespace Messenger.Application.Features.Users.DTO
{
    public sealed record SearchUserResponse(
        Guid Id,
        string Name,
        string Username,
        string? IconUri);
}
