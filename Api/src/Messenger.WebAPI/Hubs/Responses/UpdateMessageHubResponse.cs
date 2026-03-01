namespace Messenger.WebAPI.Hubs.Responses
{
    public sealed record UpdateMessageHubResponse(
        Guid ChatId,
        Guid MessageId,
        string NewContent,
        DateTime UpdatedAt);
}
