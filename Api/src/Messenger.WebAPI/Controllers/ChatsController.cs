using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.Commands.CreateGroupChat;
using Messenger.Application.Features.Chats.Commands.CreatePrivateChat;
using Messenger.Application.Features.Chats.Commands.SendMessage;
using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.Application.Features.Chats.Queries.GetById;
using Messenger.Application.Features.Chats.Queries.GetChatMessagesPaginated;
using Messenger.Application.Features.Chats.Queries.GetChatWithUserExists;
using Messenger.Application.Features.Chats.Queries.GetCurrentUserChats;
using Messenger.Application.Features.Chats.Queries.GetCurrentUserChatsPaginated;
using Messenger.WebAPI.CommandWrappers.CreateGroupChat;
using Messenger.WebAPI.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

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

        [HttpGet("/api/private-chats/exists")]
        public async Task<IActionResult> GetPrivateChatExistsBetweenUsers(
            [FromServices] IQueryHandler<GetChatWithUserExistsQuery, ChatExistsResponse> queryHandler,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken)
        {
            var query = new GetChatWithUserExistsQuery(userId);

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

            return commandResult.IsSuccess ? Ok(commandResult.Value) : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("/api/private-chats")]
        public async Task<IActionResult> CreatePrivateChat(
            [FromServices] ICommandHandler<CreatePrivateChatCommand, Guid> commandHandler,
            [FromBody] CreatePrivateChatCommand command,
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

        [HttpPost("/api/group-chats")]
        [Consumes(Multipart.FormData)]
        public async Task<IActionResult> CreateGroupChat(
            [FromServices] ICommandHandler<CreateGroupChatCommand, Guid> commandHandler,
            [FromForm] CreateGroupChatCommandWrapper wrapper,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(wrapper.ToCommand(), cancellationToken);

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
