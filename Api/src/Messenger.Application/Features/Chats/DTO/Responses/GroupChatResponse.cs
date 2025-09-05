using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record GroupChatResponse(
        Guid Id,
        DateTime CreationDate,
        MessageResponse LastMessage,
        List<GroupMemberResponse> Participants) : ChatResponse(Id, CreationDate, LastMessage);
}
