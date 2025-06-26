using Messenger.Application.Abstractions.Messaging;

namespace Messenger.Application.Features.Chats.Commands.Create
{
    public sealed record CreateChatCommand(
        Guid InviteeId,
        string Message) : ICommand<Guid>;
}
