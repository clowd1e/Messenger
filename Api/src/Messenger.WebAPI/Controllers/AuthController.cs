using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Auth.Commands.ConfirmEmail;
using Messenger.Application.Features.Auth.Commands.Login;
using Messenger.Application.Features.Auth.Commands.RefreshToken;
using Messenger.Application.Features.Auth.Commands.Register;
using Messenger.Application.Features.Auth.Commands.RequestPasswordRecovery;
using Messenger.Application.Features.Auth.Commands.ResetPassword;
using Messenger.Application.Features.Auth.DTO.Response;
using Messenger.Application.Features.Auth.Queries.ValidateEmailConfirmation;
using Messenger.Application.Features.Auth.Queries.ValidatePasswordRecovery;
using Messenger.WebAPI.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController(
        ProblemDetailsFactory problemDetailsFactory) : ControllerBase
    {
        #region Queries

        [HttpGet("validate-email-confirmation")]
        public async Task<IActionResult> ValidateEmailConfirmationAsync(
            [FromServices] IQueryHandler<ValidateEmailConfirmationQuery, ValidateEmailConfirmationResponse> queryHandler,
            [FromQuery] Guid userId,
            [FromQuery] Guid tokenId,
            CancellationToken cancellationToken)
        {
            var query = new ValidateEmailConfirmationQuery(userId, tokenId);

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }

        [HttpGet("validate-password-recovery")]
        public async Task<IActionResult> ValidatePasswordRecoveryAsync(
            [FromServices] IQueryHandler<ValidatePasswordRecoveryQuery, ValidatePasswordRecoveryResponse> queryHandler,
            [FromQuery] Guid userId,
            [FromQuery] Guid tokenId,
            CancellationToken cancellationToken)
        {
            var query = new ValidatePasswordRecoveryQuery(userId, tokenId);

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }

        #endregion

        #region Commands

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(
            [FromServices] ICommandHandler<RegisterCommand> commandHandler,
            [FromBody] RegisterCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(
            [FromServices] ICommandHandler<LoginCommand, LoginResponse> commandHandler,
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(
            [FromServices] ICommandHandler<RefreshTokenCommand, RefreshTokenResponse> commandHandler,
            [FromBody] RefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromServices] ICommandHandler<ConfirmEmailCommand> commandHandler,
            [FromBody] ConfirmEmailCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("request-password-recovery")]
        public async Task<IActionResult> RequestPasswordRecoveryAsync(
            [FromServices] ICommandHandler<RequestPasswordRecoveryCommand> commandHandler,
            [FromBody] RequestPasswordRecoveryCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(
            [FromServices] ICommandHandler<ResetPasswordCommand> commandHandler,
            [FromBody] ResetPasswordCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        #endregion
    }
}
