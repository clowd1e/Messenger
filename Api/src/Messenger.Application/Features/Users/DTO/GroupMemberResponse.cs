using Messenger.Domain.Aggregates.GroupChats;

namespace Messenger.Application.Features.Users.DTO
{
    public sealed record GroupMemberResponse(
        ShortUserResponse User,
        GroupRole Role);
}
