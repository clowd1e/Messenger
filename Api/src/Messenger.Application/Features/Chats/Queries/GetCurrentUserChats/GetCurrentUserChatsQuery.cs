using MediatR;
using Messenger.Application.Features.Chats.DTO;

namespace Messenger.Application.Features.Chats.Queries.GetCurrentUserChats
{
    public sealed record GetCurrentUserChatsQuery()
        : IRequest<Result<IEnumerable<ChatResponse>>>;
}
