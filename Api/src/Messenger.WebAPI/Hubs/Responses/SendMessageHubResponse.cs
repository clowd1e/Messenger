using Messenger.Application.Features.Chats.DTO.Responses;

namespace Messenger.WebAPI.Hubs.Responses
{
    public sealed record SendMessageHubResponse(
        Guid ChatId,
        MessageResponse Message);
}
