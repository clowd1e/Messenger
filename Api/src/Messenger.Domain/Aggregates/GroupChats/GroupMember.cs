using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.GroupChats
{
    public sealed class GroupMember
    {
        private GroupMember() { }

        private GroupMember(
            Users.User user,
            GroupRole role)
        {
            User = user;
            Role = role;
        }

        public GroupRole Role { get; private set; }

        public Users.User? User { get; private set; } = default;

        public GroupChat? GroupChat { get; private set; } = default;

        public Result AddChat(Chat chat)
        {
            if (chat is not GroupChat groupChat)
            {
                return Result.Failure(ChatErrors.InvalidChatType);
            }

            if (GroupChat is not null)
            {
                return Result.Failure(ChatErrors.UserAlreadyInChat);
            }

            GroupChat = groupChat;

            return Result.Success();
        }

        public static Result<GroupMember> Create(
            Users.User user,
            GroupRole role)
        {
            return new GroupMember(user, role);
        }
    }
}
