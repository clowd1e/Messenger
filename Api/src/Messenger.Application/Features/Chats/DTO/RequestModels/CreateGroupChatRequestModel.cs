using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Chats.DTO.RequestModels
{
    public sealed record CreateGroupChatRequestModel(
        string Name,
        string? Description,
        User Inviter,
        User[] Invitees);
}
