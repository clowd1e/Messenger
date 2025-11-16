using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.Responses;

namespace Messenger.Application.Features.Chats.Queries.GetChatWithUserExists
{
    public sealed record GetChatWithUserExistsQuery(
        Guid UserId) : IQuery<ChatExistsResponse>;
}
