using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record MessageResponse(
        Guid Id,
        ShortUserResponse Sender,
        DateTime Timestamp,
        string Content);
}
