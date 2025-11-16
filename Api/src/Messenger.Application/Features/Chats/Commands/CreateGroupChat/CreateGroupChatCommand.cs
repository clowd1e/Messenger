using Messenger.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Features.Chats.Commands.CreateGroupChat
{
    public sealed record CreateGroupChatCommand(
        List<Guid> Invitees,
        string Name,
        string? Description,
        string Message,
        IFormFile? Icon) : ICommand<Guid>;
}
