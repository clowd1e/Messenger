using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.DTO
{
    public sealed record SendMessageRequestModel(
        string Message,
        UserId UserId);
}
