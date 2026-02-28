namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record MessageDeletedForEveryoneResponse(
        List<Guid> ParticipatnsIds,
        Guid ChatId,
        Guid MessageId);
}
