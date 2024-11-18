namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record MessageResponse(
        Guid UserId,
        DateTime Timestamp,
        string Content);
}
