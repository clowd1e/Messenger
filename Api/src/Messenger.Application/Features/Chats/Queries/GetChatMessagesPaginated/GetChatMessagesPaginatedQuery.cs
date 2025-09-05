using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.Responses;

namespace Messenger.Application.Features.Chats.Queries.GetChatMessagesPaginated
{
    public sealed record GetChatMessagesPaginatedQuery(
        Guid ChatId,
        int Page,
        int PageSize,
        DateTime RetrievalCutoff) : IQuery<PaginatedMessagesResponse>;
}
