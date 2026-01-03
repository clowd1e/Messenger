using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.Commands.SendMessage;
using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.WebAPI.Extensions;
using Messenger.WebAPI.Factories;
using Messenger.WebAPI.Hubs.Responses;
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
        private const string UserGroup = "user_";

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier!;
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{UserGroup}{userId}");
        }

        public async Task SendMessage(
            [FromServices] ICommandHandler<SendMessageCommand, SendMessageResponse> commandHandler,
            SendMessageCommand command)
        {
            var commandResult = await commandHandler.Handle(command, default);

            if (commandResult.IsFailure)
            {
                await Clients.Caller.ReceiveError(commandResult.ToProblemDetails());
                return;
            }

            var response = commandResult.Value;

            foreach (var participantId in response.ChatParticipantsIds)
            {
                var hubResponse = new SendMessageHubResponse(response.ChatId, response.Message);
                await Clients.Group($"{UserGroup}{participantId}").ReceiveUserMessage(hubResponse);
            }
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
