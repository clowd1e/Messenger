using MediatR;
using Messenger.Application.Features.Users.Queries.GetById;
using Messenger.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
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
    }
}
