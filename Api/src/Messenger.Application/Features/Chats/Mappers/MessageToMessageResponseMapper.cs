using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class MessageToMessageResponseMapper
        : Mapper<Message, MessageResponse>
    {
        public override MessageResponse Map(Message source)
        {
            return new MessageResponse(
                Sender: MapUser(source.User),
                Timestamp: source.Timestamp.Value,
                Content: source.Content.Value);
        }

        private static ShortUserResponse MapUser(User user)
        {
            return new ShortUserResponse(
                Id: user.Id.Value,
                Name: user.Name.Value,
                IconUri: user.IconUri?.Value);
        }
    }
}
