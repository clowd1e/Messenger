using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;

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

        private List<UserResponse> MapUsers(
            IReadOnlyCollection<User> users)
        {
            List<UserResponse> result = new();

            foreach (var user in users)
            {
                var userResponse = new UserResponse(
                    Id: user.Id.Value,
                    Username: user.Username.Value,
                    Email: user.Email.Value);

                result.Add(userResponse);
            }

            return result;
        }
    }
}
