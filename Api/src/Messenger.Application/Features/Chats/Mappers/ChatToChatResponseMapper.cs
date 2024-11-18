using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class ChatToChatResponseMapper
        : Mapper<Chat, ChatResponse>
    {
        public override ChatResponse Map(Chat source)
        {
            var messages = MapMessages(source.Messages);

            return new(
                Id: source.Id.Value,
                CreationDate: source.CreationDate.Value,
                Messages: messages);
        }

        private List<MessageResponse> MapMessages(
            IReadOnlyCollection<Message> messages)
        {
            List<MessageResponse> result = new();

            foreach (var message in messages)
            {
                var messageResponse = new MessageResponse(
                    UserId: message.UserId.Value,
                    Timestamp: message.Timestamp.Value,
                    Content: message.Content.Value);

                result.Add(messageResponse);
            }

            return result;
        }
    }
}
