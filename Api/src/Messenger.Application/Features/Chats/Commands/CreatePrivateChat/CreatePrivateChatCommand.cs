using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.Responses;

namespace Messenger.Application.Features.Chats.Commands.CreatePrivateChat
{
    public sealed record CreatePrivateChatCommand(
        Guid InviteeId,
        string Message) : ICommand<PrivateChatResponse>;
}
