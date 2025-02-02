using MediatR;
using Messenger.Application.Features.Users.Commands.RemoveIcon;
using Messenger.Application.Features.Users.Commands.SetIcon;
using Messenger.Application.Features.Users.Queries.GetAll;
using Messenger.Application.Features.Users.Queries.GetAllExceptCurrent;
using Messenger.Application.Features.Users.Queries.GetById;
using Messenger.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public sealed class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(
            [FromRoute] Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserByIdQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var queryResult = await _sender.Send(new GetAllUsersQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("except-current")]
        public async Task<IActionResult> GetAllUsersExceptCurrent()
        {
            var queryResult = await _sender.Send(new GetAllUsersExceptCurrentQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpPut("set-icon")]
        public async Task<IActionResult> SetUserIcon(
            [FromForm] SetUserIconCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("remove-icon")]
        public async Task<IActionResult> RemoveUserIcon()
        {
            var commandResult = await _sender.Send(new RemoveUserIconCommand());

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }
    }
}
