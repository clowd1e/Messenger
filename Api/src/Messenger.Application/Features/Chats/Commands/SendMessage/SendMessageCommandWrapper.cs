using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.SendMessage
{
    internal sealed record SendMessageCommandWrapper(
        SendMessageCommand Command,
        UserId UserId);
}
