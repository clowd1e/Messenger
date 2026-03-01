namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record UpdateMessageResponse(
        List<Guid> ParticipatnsIds,
        Guid ChatId,
        Guid MessageId,
        string NewContent,
        DateTime UpdatedAt);
}