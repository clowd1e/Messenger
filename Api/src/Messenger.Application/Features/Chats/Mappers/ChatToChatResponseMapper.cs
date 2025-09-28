using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.GroupChats;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class ChatToChatResponseMapper
        : Mapper<Chat, ChatResponse>
    {
        public override ChatResponse Map(Chat source)
        {
            var lastMessage = MapLastMessage(source.Messages);

            return source switch
            {
                PrivateChat privateChat => MapPrivateChat(privateChat, lastMessage),
                GroupChat groupChat => MapGroupChat(groupChat, lastMessage),
                _ => throw new InvalidOperationException(
                    $"Unsupported chat type: {source.GetType().Name}"),
            };
        }

        private static GroupChatResponse MapGroupChat(
            GroupChat groupChat,
            MessageResponse lastMessage)
        {
            return new GroupChatResponse(
                Id: groupChat.Id.Value,
                CreationDate: groupChat.CreationDate.Value,
                Name: groupChat.Name.Value,
                Description: groupChat.Description?.Value,
                IconUri: groupChat.IconUri?.Value,
                LastMessage: lastMessage,
                Participants: MapGroupMembers(groupChat.GroupMembers));
        }

        private static PrivateChatResponse MapPrivateChat(
            PrivateChat privateChat,
            MessageResponse lastMessage)
        {
            return new PrivateChatResponse(
                Id: privateChat.Id.Value,
                CreationDate: privateChat.CreationDate.Value,
                LastMessage: lastMessage,
                Participants: MapUsers(privateChat.Participants));
        }

        private static MessageResponse MapLastMessage(
            IReadOnlyCollection<Message> messages)
        {
            var message = messages.OrderByDescending(m => m.Timestamp.Value).First();

            return new MessageResponse(
                Id: message.Id.Value,
                Sender: MapUser(message.User),
                Timestamp: message.Timestamp.Value,
                Content: message.Content.Value);
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

        private static List<GroupMemberResponse> MapGroupMembers(
            IReadOnlyCollection<GroupMember> groupMembers)
        {
            List<GroupMemberResponse> result = [];

            foreach (var member in groupMembers)
            {
                var memberResponse = MapGroupMember(member);

                result.Add(memberResponse);
            }

            return result;
        }

        private static GroupMemberResponse MapGroupMember(
            GroupMember member)
        {
            return new GroupMemberResponse(
                User: MapUser(member.User),
                Role: member.Role);
        }
    }
}
