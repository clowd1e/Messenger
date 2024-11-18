using MediatR;
using Messenger.Application.Features.Chats.DTO;

namespace Messenger.Application.Features.Chats.Queries.GetById
{
    public sealed record GetChatByIdQuery(
        Guid? ChatId) : IRequest<Result<ChatResponse>>;
}
