using System.Text.Json.Serialization;

namespace Messenger.Application.Features.Chats.DTO.Responses
{
    public abstract record ChatResponse(
        Guid Id,
        DateTime CreationDate,
        MessageResponse LastMessage);
}
