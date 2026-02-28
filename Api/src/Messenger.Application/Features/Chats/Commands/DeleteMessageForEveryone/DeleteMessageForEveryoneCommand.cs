using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.Responses;

namespace Messenger.Application.Features.Chats.Commands.DeleteMessageForEveryone
{
    public sealed record DeleteMessageForEveryoneCommand(
        Guid ChatId,
        Guid MessageId) : ICommand<MessageDeletedForEveryoneResponse>;
}
