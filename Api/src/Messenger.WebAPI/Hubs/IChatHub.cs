using Messenger.Application.Features.Chats.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Hubs
{
    public interface IChatHub
    {
        Task ReceiveError(ProblemDetails error);

        Task ReceiveUserChats(IEnumerable<ChatResponse> chats);

        Task ReceiveUserMessage(MessageResponse message);

        Task ReceiveChat(ChatResponse chat);
    }
}
