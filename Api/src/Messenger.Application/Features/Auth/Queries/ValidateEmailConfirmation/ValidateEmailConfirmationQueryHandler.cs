using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Auth.DTO.Response;
using Messenger.Domain.Aggregates.ConfirmEmailTokens;
using Messenger.Domain.Aggregates.ConfirmEmailTokens.Errors;
using Messenger.Domain.Aggregates.ConfirmEmailTokens.ValueObjects;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Queries.ValidateEmailConfirmation
{
    internal sealed class ValidateEmailConfirmationQueryHandler
        : IQueryHandler<ValidateEmailConfirmationQuery, ValidateEmailConfirmationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfirmEmailTokenRepository _confirmEmailTokenRepository;

        public ValidateEmailConfirmationQueryHandler(
            IUserRepository userRepository,
            IConfirmEmailTokenRepository confirmEmailTokenRepository)
        {
            _userRepository = userRepository;
            _confirmEmailTokenRepository = confirmEmailTokenRepository;
        }

        public async Task<Result<ValidateEmailConfirmationResponse>> Handle(
            ValidateEmailConfirmationQuery request,
            CancellationToken cancellationToken)
        {
            // Check if the user exists

            var userId = new UserId(request.UserId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<ValidateEmailConfirmationResponse>(
                    UserErrors.NotFound);
            }

            // Check if the token exists

            var tokenId = new ConfirmEmailTokenId(request.TokenId);

            var token = await _confirmEmailTokenRepository.GetByIdWithUserAsync(tokenId, cancellationToken);

            if (token is null)
            {
                return Result.Failure<ValidateEmailConfirmationResponse>(
                    ConfirmEmailTokenErrors.NotFound);
            }

            // Check if user has the token

            if (token.User.Id != userId)
            {
                return Result.Failure<ValidateEmailConfirmationResponse>(
                    ConfirmEmailTokenErrors.TokenNotAssignedToUser);
            }

            // Check if the token is already used

            if (token.IsUsed)
            {
                return Result.Failure<ValidateEmailConfirmationResponse>(
                    ConfirmEmailTokenErrors.AlreadyUsed);
            }

            // Check if the token is expired

            if (token.ExpiresAt.Value < DateTime.UtcNow)
            {
                return Result.Failure<ValidateEmailConfirmationResponse>(
                    ConfirmEmailTokenErrors.Expired);
            }

            // Return the token expiration date

            var response = new ValidateEmailConfirmationResponse(
                token.ExpiresAt.Value);

            return Result.Success(response);
        }
    }
}
