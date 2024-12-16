using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Chats.Commands.Create
{
    public sealed record CreateChatCommandWrapper(
        User Inviter,
        User Invitee);
}
