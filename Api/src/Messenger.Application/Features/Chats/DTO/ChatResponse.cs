using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record ChatResponse(
        Guid Id,
        DateTime CreationDate,
        MessageResponse LastMessage,
        List<ShortUserResponse> Users);
}
