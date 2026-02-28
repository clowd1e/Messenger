namespace Messenger.WebAPI.Hubs.Responses
{
    public sealed record DeleteMessageHubResponse(
        Guid ChatId,
        Guid MessageId);
}
