using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.WebAPI.Hubs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Hubs
{
    public interface IChatHub
    {
        Task ReceiveError(ProblemDetails error);

        Task ReceiveUserChats(IEnumerable<ChatResponse> chats);

        Task ReceiveUserMessage(SendMessageHubResponse response);

        Task ReceiveChat(ChatResponse chat);

        Task DeleteMessage(DeleteMessageHubResponse response);
    }
}
