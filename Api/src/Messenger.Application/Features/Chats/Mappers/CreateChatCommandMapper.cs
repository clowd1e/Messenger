using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.ValueObjects;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class CreateChatCommandMapper
        : Mapper<CreateChatRequestModel, Result<Chat>>
    {
        public override Result<Chat> Map(CreateChatRequestModel source)
        {
            return Chat.Create(
                chatId: new ChatId(Guid.NewGuid()),
                creationDate: ChatCreationDate.UtcNow().Value,
                users: [source.Inviter, source.Invitee]);
        }
    }
}
