using MediatR;
using Messenger.Application.Features.Chats.Commands.SendMessage;
using Messenger.Application.Features.Users.Queries.GetCurrentUserChats;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public sealed class ChatHub : Hub<IChatHub>
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
                await Clients.Caller.ReceiveError(queryResult.Error);
                return;
            }

            var chats = queryResult.Value;

            await Clients.Caller.ReceiveUserChats(chats);
        }

        public async Task SendUserMessage(SendMessageCommand command)
        {
            var commandResult = await _sender.Send(command);

            if (commandResult.IsFailure)
            {
                await Clients.Caller.ReceiveError(commandResult.Error);
                return;
            }

            var response = commandResult.Value;

            await Clients.Group(command.ChatId.ToString()).ReceiveUserMessage(response);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            
        }
    }
}
