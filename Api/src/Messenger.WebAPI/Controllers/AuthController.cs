using MediatR;
using Messenger.Application.Features.Auth.Commands.ConfirmEmail;
using Messenger.Application.Features.Auth.Commands.Login;
using Messenger.Application.Features.Auth.Commands.RefreshToken;
using Messenger.Application.Features.Auth.Commands.Register;
using Messenger.Application.Features.Auth.Commands.RequestPasswordRecovery;
using Messenger.Application.Features.Auth.Commands.ResetPassword;
using Messenger.Application.Features.Auth.Queries.ValidateEmailConfirmation;
using Messenger.Application.Features.Auth.Queries.ValidatePasswordRecovery;
using Messenger.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries

        [HttpGet("validate-email-confirmation")]
        public async Task<IActionResult> ValidateEmailConfirmationAsync(
            [FromQuery] Guid userId,
            [FromQuery] Guid tokenId,
            CancellationToken cancellationToken)
        {
            var query = new ValidateEmailConfirmationQuery(userId, tokenId);

            var queryResult = await _sender.Send(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToActionResult();
        }

        [HttpGet("validate-password-recovery")]
        public async Task<IActionResult> ValidatePasswordRecoveryAsync(
            [FromQuery] Guid userId,
            [FromQuery] Guid tokenId,
            CancellationToken cancellationToken)
        {
            var query = new ValidatePasswordRecoveryQuery(userId, tokenId);

            var queryResult = await _sender.Send(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToActionResult();
        }

        #endregion

        #region Commands

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await _sender.Send(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToActionResult();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await _sender.Send(command, cancellationToken);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : commandResult.ToActionResult();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(
            [FromBody] RefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await _sender.Send(command, cancellationToken);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : commandResult.ToActionResult();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromBody] ConfirmEmailCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await _sender.Send(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToActionResult();
        }

        [HttpPost("request-password-recovery")]
        public async Task<IActionResult> RequestPasswordRecoveryAsync(
            [FromBody] RequestPasswordRecoveryCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await _sender.Send(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToActionResult();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(
            [FromBody] ResetPasswordCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await _sender.Send(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToActionResult();
        }

        #endregion
    }
}
