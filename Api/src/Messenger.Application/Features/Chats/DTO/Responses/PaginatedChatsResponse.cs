namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record PaginatedChatsResponse(
        IEnumerable<ChatResponse> Chats,
        bool IsLastPage);
}
