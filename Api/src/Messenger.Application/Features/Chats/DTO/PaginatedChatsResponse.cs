namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record PaginatedChatsResponse(
        IEnumerable<ShortChatResponse> Chats,
        bool IsLastPage);
}
