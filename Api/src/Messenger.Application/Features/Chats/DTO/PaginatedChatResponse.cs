namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record PaginatedChatResponse(
        IEnumerable<ShortChatResponse> Chats,
        bool IsLastPage);
}
