using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record GroupChatResponse(
        Guid Id,
        DateTime CreationDate,
        string Name,
        string? Description,
        MessageResponse LastMessage,
        List<GroupMemberResponse> Participants) : ChatResponse(Id, CreationDate, LastMessage);
}
