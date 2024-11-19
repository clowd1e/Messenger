using MediatR;
using Messenger.Application.Features.Auth.Commands.Register;
using Messenger.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await _sender.Send(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }
    }
}
