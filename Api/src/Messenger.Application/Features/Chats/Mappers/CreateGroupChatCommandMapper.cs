using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO.RequestModels;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Common.ImageUri;
using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.GroupChats;
using Messenger.Domain.Aggregates.GroupChats.ValueObjects;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class CreateGroupChatCommandMapper
        : Mapper<CreateGroupChatRequestModel, Result<GroupChat>>
    {
        public override Result<GroupChat> Map(CreateGroupChatRequestModel source)
        {
            var adminResult = GroupMember.Create(source.Inviter, GroupRole.Admin);
            if (adminResult.IsFailure)
            {
                return Result.Failure<GroupChat>(adminResult.Error);
            }

            var admin = adminResult.Value;

            List<GroupMember> participants = [admin];

            foreach (var invitee in source.Invitees)
            {
                var memberResult = GroupMember.Create(invitee, GroupRole.Member);
                if (memberResult.IsFailure)
                {
                    return Result.Failure<GroupChat>(memberResult.Error);
                }

                participants.Add(memberResult.Value);
            }

            var chatId = new ChatId(Guid.NewGuid());

            var nameResult = GroupChatName.Create(source.Name);
            if (nameResult.IsFailure)
            {
                return Result.Failure<GroupChat>(nameResult.Error);
            }

            var name = nameResult.Value;

            GroupChatDescription? description = null;

            if (source.Description is not null)
            {
                var descriptionResult = GroupChatDescription.Create(source.Description);
                if (descriptionResult.IsFailure)
                {
                    return Result.Failure<GroupChat>(descriptionResult.Error);
                }

                description = descriptionResult.Value;
            }

            var creationDateResult = Timestamp.UtcNow();
            if (creationDateResult.IsFailure)
            {
                return Result.Failure<GroupChat>(creationDateResult.Error);
            }

            var creationDate = creationDateResult.Value;

            return GroupChat.Create(
                chatId: chatId,
                name: name,
                creationDate: creationDate,
                description: description,
                iconUri: source.IconUri,
                groupMembers: participants);
        }
    }
}
