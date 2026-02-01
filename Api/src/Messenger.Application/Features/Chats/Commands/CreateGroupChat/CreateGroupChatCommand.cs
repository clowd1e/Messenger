using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.Responses;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Features.Chats.Commands.CreateGroupChat
{
    public sealed record CreateGroupChatCommand(
        List<Guid> Invitees,
        string Name,
        string? Description,
        string Message,
        IFormFile? Icon) : ICommand<GroupChatResponse>;
}
