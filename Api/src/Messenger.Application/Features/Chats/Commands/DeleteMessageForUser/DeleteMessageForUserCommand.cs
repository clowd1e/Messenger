using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.Responses;

namespace Messenger.Application.Features.Chats.Commands.DeleteMessageForUser
{
    public sealed record DeleteMessageForUserCommand(
        Guid ChatId,
        Guid MessageId) : ICommand<MessageDeletedForUserResponse>;
}
