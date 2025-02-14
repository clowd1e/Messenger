using MediatR;
using Messenger.Application.Features.Chats.DTO;

namespace Messenger.Application.Features.Chats.Queries.GetChatMessagesPaginated
{
    public sealed record GetChatMessagesPaginatedQuery(
        Guid ChatId,
        int Page,
        int PageSize,
        DateTime RetrievalCutoff) : IRequest<Result<PaginatedMessagesResponse>>;
}
