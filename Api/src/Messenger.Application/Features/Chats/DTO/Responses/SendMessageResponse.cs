namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record SendMessageResponse(
        List<Guid> ChatParticipantsIds,
        Guid ChatId,
        MessageResponse Message);
}
