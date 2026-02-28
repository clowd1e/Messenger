namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public sealed record MessageDeletedForUserResponse(
        Guid ChatId,
        Guid MessageId,
        Guid UserId);
}
