using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record ShortChatResponse(
        Guid Id,
        DateTime CreationDate,
        MessageResponse LastMessage,
        List<ShortUserResponse> Users);
}
