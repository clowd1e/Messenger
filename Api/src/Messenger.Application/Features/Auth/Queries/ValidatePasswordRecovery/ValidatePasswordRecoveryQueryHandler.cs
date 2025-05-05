using MediatR;
using Messenger.Application.Features.Auth.DTO;
using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Messenger.Domain.Aggregates.ResetPasswordTokens.Errors;
using Messenger.Domain.Aggregates.ResetPasswordTokens.ValueObjects;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Queries.ValidatePasswordRecovery
{
    internal sealed class ValidatePasswordRecoveryQueryHandler
        : IRequestHandler<ValidatePasswordRecoveryQuery, Result<ValidatePasswordRecoveryResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IResetPasswordTokenRepository _resetPasswordTokenRepository;

        public ValidatePasswordRecoveryQueryHandler(
            IUserRepository userRepository,
            IResetPasswordTokenRepository resetPasswordTokenRepository)
        {
            _userRepository = userRepository;
            _resetPasswordTokenRepository = resetPasswordTokenRepository;
        }

        public async Task<Result<ValidatePasswordRecoveryResponse>> Handle(
            ValidatePasswordRecoveryQuery request,
            CancellationToken cancellationToken)
        {
            // Check if the user exists

            var userId = new UserId(request.UserId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    UserErrors.NotFound);
            }

            // Check if the token exists

            var tokenId = new ResetPasswordTokenId(request.TokenId);

            var token = await _resetPasswordTokenRepository.GetByIdWithUserAsync(tokenId, cancellationToken);

            if (token is null)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ResetPasswordTokenErrors.NotFound);
            }

            // Check if user has the token

            if (token.User.Id != userId)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ResetPasswordTokenErrors.TokenNotAssignedToUser);
            }

            // Check if the token is already used

            if (token.IsUsed)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ResetPasswordTokenErrors.AlreadyUsed);
            }

            // Check if the token is expired

            if (token.ExpiresAt.Value < DateTime.UtcNow)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ResetPasswordTokenErrors.Expired);
            }

            // Return the token expiration date

            var response = new ValidatePasswordRecoveryResponse(
                token.ExpiresAt.Value);

            return Result.Success(response);
        }
    }
}
