using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.Commands.Create;
using Messenger.Application.Features.Chats.Commands.SendMessage;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Application.Features.Chats.Queries.GetById;
using Messenger.WebAPI.Extensions;
using Messenger.WebAPI.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public sealed class ChatHub(
        ProblemDetailsFactory problemDetailsFactory) : Hub<IChatHub>
    {
        public override async Task OnConnectedAsync()
        {
        }

        public async Task CreateChat(
            [FromServices] ICommandHandler<CreateChatCommand, Guid> commandHandler,
            [FromServices] IQueryHandler<GetChatByIdQuery, ChatResponse> queryHandler,
            CreateChatCommand command)
        {
            var commandResult = await commandHandler.Handle(command, default);

            if (commandResult.IsFailure)
            {
                await Clients.Caller.ReceiveError(commandResult.ToProblemDetails());
                return;
            }

            var chatId = commandResult.Value;

            var query = new GetChatByIdQuery(chatId);

            var queryResult = await queryHandler.Handle(query, default);

            if (queryResult.IsFailure)
            {
                await Clients.Caller.ReceiveError(queryResult.ToProblemDetails());
                return;
            }

            var chat = queryResult.Value;

            await Clients.User(Context.ConnectionId).ReceiveChat(chat);
            await Clients.User(command.InviteeId.ToString()).ReceiveChat(chat);
        }

        public async Task SendMessage(
            [FromServices] ICommandHandler<SendMessageCommand, MessageResponse> commandHandler,
            SendMessageCommand command)
        {
            var commandResult = await commandHandler.Handle(command, default);

            if (commandResult.IsFailure)
            {
                await Clients.Caller.ReceiveError(commandResult.ToProblemDetails());
                return;
            }

            var response = commandResult.Value;

            await Clients.Group(command.ChatId.ToString()).ReceiveUserMessage(response);
        }

        public async Task JoinChat(Guid chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

        }
    }
}
