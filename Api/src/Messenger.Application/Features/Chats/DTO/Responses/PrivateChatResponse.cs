using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record PrivateChatResponse(
        Guid Id,
        DateTime CreationDate,
        MessageResponse LastMessage,
        List<ShortUserResponse> Participants) : ChatResponse(Id, CreationDate, LastMessage);
}
