using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.Responses;

namespace Messenger.Application.Features.Chats.Commands.UpdateMessage
{
    public sealed record UpdateMessageCommand(
        Guid ChatId,
        Guid MessageId,
        string NewContent) : ICommand<UpdateMessageResponse>;
}
