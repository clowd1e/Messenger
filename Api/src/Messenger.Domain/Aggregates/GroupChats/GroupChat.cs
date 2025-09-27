using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Common.ImageUri;
using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.GroupChats;
using Messenger.Domain.Aggregates.GroupChats.ValueObjects;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats
{
    public sealed class GroupChat : Chat
    {
        public const int MinParticipantsCount = 3;
        public const int MaxParticipantsCount = 20;

        private readonly HashSet<GroupMember> _groupMembers = [];
        private GroupChatName _name;

        private GroupChat()
            : base() { }

        private GroupChat(
            ChatId chatId,
            GroupChatName name,
            Timestamp creationDate,
            GroupChatDescription? description,
            ImageUri? iconUri)
            : base(chatId, creationDate)
        {
            Name = name;
            Description = description;
            IconUri = iconUri;
        }

        public GroupChatName Name
        {
            get => _name;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);
                _name = value;
            }
        }

        public GroupChatDescription? Description { get; private set; }

        public ImageUri? IconUri { get; private set; }

        public IReadOnlyCollection<GroupMember> GroupMembers => _groupMembers;

        public Result AddMember(GroupMember member)
        {
            if (member.User is null)
            {
                return Result.Failure(
                    ChatErrors.InvalidGroupMember);
            }

            if (Participants.Count >= MaxParticipantsCount)
            {
                return Result.Failure(
                    ChatErrors.MaxParticipantsLimit(MaxParticipantsCount));
            }

            if (Participants.Any(participant => participant.Id == member.User.Id))
            {
                return Result.Failure(
                    ChatErrors.UserAlreadyInChat);
            }

            _participants.Add(member.User);
            _groupMembers.Add(member);

            return Result.Success();
        }

        public Result RemoveMember(GroupMember member)
        {
            if (member.User is null)
            {
                return Result.Failure(
                    ChatErrors.InvalidGroupMember);
            }

            if (!_participants.Any(participant => participant.Id == member.User.Id))
            {
                return Result.Failure(
                    ChatErrors.UserNotInChat);
            }

            _participants.Remove(member.User);
            _groupMembers.Remove(member);

            return Result.Success();
        }

        public static Result<GroupChat> Create(
            ChatId chatId,
            GroupChatName name,
            Timestamp creationDate,
            GroupChatDescription? description,
            ImageUri? iconUri,
            List<GroupMember> groupMembers)
        {
            if (groupMembers.Count < MinParticipantsCount)
            {
                return Result.Failure<GroupChat>(
                    ChatErrors.MinParticipantsLimit(MinParticipantsCount));
            }

            if (groupMembers.Count > MaxParticipantsCount)
            {
                return Result.Failure<GroupChat>(
                    ChatErrors.MaxParticipantsLimit(MaxParticipantsCount));
            }

            var groupChat = new GroupChat(
                chatId,
                name,
                creationDate,
                description,
                iconUri);

            List<Users.User> participants = [.. groupMembers
                .Select(member => member.User ?? throw new InvalidOperationException("Group member must have a user."))];

            groupChat.SetParticipants(participants);
            groupChat.SetGroupMembers(groupMembers);

            if (groupChat.Participants.Count != groupChat.GroupMembers.Count)
            {
                throw new InvalidOperationException("Inserted participants count is not the same as group mebmers count");
            }

            return groupChat;
        }

        private void SetGroupMembers(List<GroupMember> groupMembers)
        {
            foreach (var groupMember in groupMembers) 
            {
                _groupMembers.Add(groupMember);
            }
        }
    }
}
