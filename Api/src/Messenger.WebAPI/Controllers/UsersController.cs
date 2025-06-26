using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Users.Commands.RemoveIcon;
using Messenger.Application.Features.Users.Commands.SetIcon;
using Messenger.Application.Features.Users.DTO;
using Messenger.Application.Features.Users.Queries.GetAll;
using Messenger.Application.Features.Users.Queries.GetAllExceptCurrent;
using Messenger.Application.Features.Users.Queries.GetById;
using Messenger.WebAPI.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public sealed class UsersController(
        ProblemDetailsFactory problemDetailsFactory) : ControllerBase
    {
        #region Queries

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(
            [FromServices] IQueryHandler<GetUserByIdQuery, UserResponse> queryHandler,
            [FromRoute] Guid userId,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(userId);

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
            [FromServices] IQueryHandler<GetAllUsersQuery, IEnumerable<UserResponse>> queryHandler,
            CancellationToken cancellationToken)
        {
            var query = new GetAllUsersQuery();

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }

        [HttpGet("except-current")]
        public async Task<IActionResult> GetAllUsersExceptCurrent(
            [FromServices] IQueryHandler<GetAllUsersExceptCurrentQuery, IEnumerable<ShortUserResponse>> queryHandler,
            CancellationToken cancellationToken)
        {
            var query = new GetAllUsersExceptCurrentQuery();

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }

        #endregion

        #region Commands

        [HttpPut("set-icon")]
        public async Task<IActionResult> SetUserIcon(
            [FromServices] ICommandHandler<SetUserIconCommand> commandHandler,
            [FromForm] SetUserIconCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPut("remove-icon")]
        public async Task<IActionResult> RemoveUserIcon(
            [FromServices] ICommandHandler<RemoveUserIconCommand> commandHandler,
            CancellationToken cancellationToken)
        {
            var command = new RemoveUserIconCommand();

            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        #endregion
    }
}
