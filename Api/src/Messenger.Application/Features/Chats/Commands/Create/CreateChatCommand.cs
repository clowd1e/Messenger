using MediatR;

namespace Messenger.Application.Features.Chats.Commands.Create
{
    public sealed record CreateChatCommand(
        Guid InviteeId,
        string Message) : IRequest<Result<Guid>>;
}
