using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class ChatToShortChatResponseMapper
        : Mapper<Chat, ShortChatResponse>
    {
        public override ShortChatResponse Map(Chat source)
        {
            var lastMessage = MapLastMessage(source.Messages);

            var users = MapUsers(source.Users);

            return new(
                Id: source.Id.Value,
                CreationDate: source.CreationDate.Value,
                LastMessage: lastMessage,
                Users: users);
        }

        private static MessageResponse MapLastMessage(
            IReadOnlyCollection<Message> messages)
        {
            var message = messages.OrderByDescending(m => m.Timestamp.Value).First();

            return new MessageResponse(
                UserId: message.UserId.Value,
                Timestamp: message.Timestamp.Value,
                Content: message.Content.Value);
        }

        private static List<UserResponse> MapUsers(
            IReadOnlyCollection<User> users)
        {
            List<UserResponse> result = [];

            foreach (var user in users)
            {
                var userResponse = new UserResponse(
                    Id: user.Id.Value,
                    Username: user.Username.Value,
                    Name: user.Name.Value,
                    Email: user.Email.Value,
                    IconUri: user.IconUri?.Value);

                result.Add(userResponse);
            }

            return result;
        }
    }
}
