using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class ChatToChatResponseMapper
        : Mapper<Chat, ChatResponse>
    {
        public override ChatResponse Map(Chat source)
        {
            var messages = MapMessages(source.Messages);

            var users = MapUsers(source.Users);

            return new(
                Id: source.Id.Value,
                CreationDate: source.CreationDate.Value,
                Messages: messages,
                Users: users);
        }

        private static List<MessageResponse> MapMessages(
            IReadOnlyCollection<Message> messages)
        {
            List<MessageResponse> result = [];

            foreach (var message in messages)
            {
                var messageResponse = new MessageResponse(
                    Sender: MapUser(message.User),
                    Timestamp: message.Timestamp.Value,
                    Content: message.Content.Value);

                result.Add(messageResponse);
            }

            return result;
        }

        private static List<ShortUserResponse> MapUsers(
            IReadOnlyCollection<User> users)
        {
            List<ShortUserResponse> result = [];

            foreach (var user in users)
            {
                var userResponse = MapUser(user);

                result.Add(userResponse);
            }

            return result;
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
