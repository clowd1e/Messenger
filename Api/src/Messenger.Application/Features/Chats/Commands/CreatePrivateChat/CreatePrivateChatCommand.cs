using Messenger.Application.Abstractions.Messaging;

namespace Messenger.Application.Features.Chats.Commands.CreatePrivateChat
{
    public sealed record CreatePrivateChatCommand(
        Guid InviteeId,
        string Message) : ICommand<Guid>;
}
