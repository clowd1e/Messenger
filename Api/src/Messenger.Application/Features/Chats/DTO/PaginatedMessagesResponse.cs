namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record PaginatedMessagesResponse(
        IEnumerable<MessageResponse> Messages,
        bool IsLastPage);
}
