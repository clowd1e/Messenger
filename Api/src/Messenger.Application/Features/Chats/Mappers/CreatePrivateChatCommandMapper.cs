using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO.RequestModels;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Common.Timestamp;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class CreatePrivateChatCommandMapper
        : Mapper<CreatePrivateChatRequestModel, Result<PrivateChat>>
    {
        public override Result<PrivateChat> Map(CreatePrivateChatRequestModel source)
        {
            return PrivateChat.Create(
                chatId: new ChatId(Guid.NewGuid()),
                creationDate: Timestamp.UtcNow(),
                participants: [source.Inviter, source.Invitee]);
        }
    }
}
