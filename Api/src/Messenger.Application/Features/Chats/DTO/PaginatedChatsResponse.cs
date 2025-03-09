namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record PaginatedChatsResponse(
        IEnumerable<ChatResponse> Chats,
        bool IsLastPage);
}
