using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class MessageToMessageResponseMapper
        : Mapper<Message, MessageResponse>
    {
        public override MessageResponse Map(Message source)
        {
            return new MessageResponse(
                UserId: source.UserId.Value,
                Timestamp: source.Timestamp.Value,
                Content: source.Content.Value);
        }
    }
}
