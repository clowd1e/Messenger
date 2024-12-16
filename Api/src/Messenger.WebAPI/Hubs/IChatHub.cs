using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Shared;

namespace Messenger.WebAPI.Hubs
{
    public interface IChatHub
    {
        Task ReceiveError(Error error);

        Task ReceiveUserChats(IEnumerable<ChatResponse> chats);

        Task ReceiveUserMessage(MessageResponse message);

        Task ReceiveChat(ChatResponse chat);
    }
}
