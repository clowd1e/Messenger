namespace Messenger.Application.Features.Users.DTO
{
    public sealed record ShortUserResponse(
        Guid Id,
        string Name,
        string? IconUri);
}
