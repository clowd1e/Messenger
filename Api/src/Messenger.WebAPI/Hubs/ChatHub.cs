using MediatR;
using Messenger.Application.Features.Users.Queries.GetCurrentUserChats;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public sealed class ChatHub : Hub
    {
        private readonly ISender _sender;

        public ChatHub(ISender sender)
        {
            _sender = sender;
        }

        public override async Task OnConnectedAsync()
        {
            var queryResult = await _sender.Send(new GetCurrentUserChatsQuery());

            if (queryResult.IsFailure)
            {
                await Clients.Caller.SendAsync("ReceiveError", queryResult.Error);
                return;
            }

            var chats = queryResult.Value;

            await Clients.Caller.SendAsync("ReceiveUserChats", chats);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            
        }
    }
}
