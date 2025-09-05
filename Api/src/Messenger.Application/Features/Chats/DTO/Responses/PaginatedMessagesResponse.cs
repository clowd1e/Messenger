namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record PaginatedMessagesResponse(
        IEnumerable<MessageResponse> Messages,
        bool IsLastPage);
}
