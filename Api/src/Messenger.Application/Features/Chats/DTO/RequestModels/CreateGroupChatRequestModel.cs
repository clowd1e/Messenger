using Messenger.Domain.Aggregates.Common.ImageUri;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Chats.DTO.RequestModels
{
    public sealed record CreateGroupChatRequestModel(
        string Name,
        string? Description,
        ImageUri? IconUri,
        User Inviter,
        User[] Invitees);
}
