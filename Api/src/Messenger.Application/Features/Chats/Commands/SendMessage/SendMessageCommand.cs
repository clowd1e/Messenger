using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO;

namespace Messenger.Application.Features.Chats.Commands.SendMessage
{
    public sealed record SendMessageCommand(
        Guid ChatId,
        string Message) : ICommand<MessageResponse>;
}
