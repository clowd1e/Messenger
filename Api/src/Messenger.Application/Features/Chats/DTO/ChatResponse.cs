namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record ChatResponse(
        Guid Id,
        DateTime CreationDate,
        List<MessageResponse> Messages);
}
