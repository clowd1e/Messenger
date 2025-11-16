using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Chats.DTO.RequestModels
{
    public sealed record CreatePrivateChatRequestModel(
        User Inviter,
        User Invitee);
}
