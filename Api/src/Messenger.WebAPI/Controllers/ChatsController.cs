using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.Commands.Create;
using Messenger.Application.Features.Chats.Commands.SendMessage;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Application.Features.Chats.Queries.GetById;
using Messenger.Application.Features.Chats.Queries.GetChatMessagesPaginated;
using Messenger.Application.Features.Chats.Queries.GetCurrentUserChats;
using Messenger.Application.Features.Chats.Queries.GetCurrentUserChatsPaginated;
using Messenger.WebAPI.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers
{
    [Route("api/chats")]
    [ApiController]
    [Authorize]
    public class ChatsController(
        ProblemDetailsFactory problemDetailsFactory) : ControllerBase
    {
        #region Queries

        [HttpGet]
        public async Task<IActionResult> GetChatsForCurrentUser(
            [FromServices] IQueryHandler<GetCurrentUserChatsQuery, IEnumerable<ChatResponse>> queryHandler,
            CancellationToken cancellationToken)
        {
            var query = new GetCurrentUserChatsQuery();

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedChatsForCurrentUser(
            [FromServices] IQueryHandler<GetCurrentUserChatsPaginatedQuery, PaginatedChatsResponse> queryHandler,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] DateTime retrievalCutoff,
            CancellationToken cancellationToken)
        {
            var query = new GetCurrentUserChatsPaginatedQuery(page, pageSize, retrievalCutoff);

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }

        [HttpGet("{chatId:guid}")]
        public async Task<IActionResult> GetChatById(
            [FromServices] IQueryHandler<GetChatByIdQuery, ChatResponse> queryHandler,
            [FromRoute] Guid chatId,
            CancellationToken cancellationToken)
        {
            var query = new GetChatByIdQuery(chatId);

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }

        [HttpGet("{chatId:guid}/messages")]
        public async Task<IActionResult> GetChatMessagesPaginated(
            [FromServices] IQueryHandler<GetChatMessagesPaginatedQuery, PaginatedMessagesResponse> queryHandler,
            [FromRoute] Guid chatId,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] DateTime retrievalCutoff,
            CancellationToken cancellationToken)
        {
            var query = new GetChatMessagesPaginatedQuery(chatId, page, pageSize, retrievalCutoff);

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }

        #endregion

        #region Commands

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(
            [FromServices] ICommandHandler<SendMessageCommand, MessageResponse> commandHandler,
            [FromBody] SendMessageCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? Ok() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(
            [FromServices] ICommandHandler<CreateChatCommand, Guid> commandHandler,
            [FromBody] CreateChatCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetChatById),
                    routeValues: new { chatId = commandResult.Value },
                    value: commandResult.Value);
            }

            return problemDetailsFactory.GetProblemDetails(commandResult);
        }

        #endregion
    }
}
