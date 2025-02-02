using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record CreateChatRequestModel(
        User Inviter,
        User Invitee);
}
