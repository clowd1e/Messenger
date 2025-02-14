using MediatR;
using Messenger.Application.Features.Chats.Commands.Create;
using Messenger.Application.Features.Chats.Commands.SendMessage;
using Messenger.Application.Features.Chats.Queries.GetById;
using Messenger.Application.Features.Chats.Queries.GetChatMessagesPaginated;
using Messenger.Application.Features.Chats.Queries.GetCurrentUserChatsPaginated;
using Messenger.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers
{
    [Route("api/chats")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly ISender _sender;

        public ChatsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries

        [HttpGet]
        public async Task<IActionResult> GetPaginatedChatsForCurrentUser(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] DateTime retrievalCutoff)
        {
            var query = new GetCurrentUserChatsPaginatedQuery(page, pageSize, retrievalCutoff);

            var queryResult = await _sender.Send(query);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{chatId:guid}")]
        public async Task<IActionResult> GetChatById(
            [FromRoute] Guid chatId)
        {
            var queryResult = await _sender.Send(new GetChatByIdQuery(chatId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{chatId:guid}/messages")]
        public async Task<IActionResult> GetChatMessagesPaginated(
            [FromRoute] Guid chatId,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] DateTime retrievalCutoff)
        {
            var query = new GetChatMessagesPaginatedQuery(chatId, page, pageSize, retrievalCutoff);

            var queryResult = await _sender.Send(query);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        #endregion

        #region Commands

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(
            [FromBody] SendMessageCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Ok() : commandResult.ToProblemDetails();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(
            [FromBody] CreateChatCommand command)
        {
            var commandResult = await _sender.Send(command);

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetChatById),
                    routeValues: new { chatId = commandResult.Value },
                    value: commandResult.Value);
            }

            return commandResult.ToProblemDetails();
        }

        #endregion
    }
}
