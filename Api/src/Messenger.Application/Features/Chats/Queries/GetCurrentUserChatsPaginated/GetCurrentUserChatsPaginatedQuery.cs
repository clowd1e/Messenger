using MediatR;
using Messenger.Application.Features.Chats.DTO;

namespace Messenger.Application.Features.Chats.Queries.GetCurrentUserChatsPaginated
{
    public sealed record GetCurrentUserChatsPaginatedQuery(
        int Page,
        int PageSize,
        DateTime RetrievalCutoff) : IRequest<Result<PaginatedChatResponse>>;
}
