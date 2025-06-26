using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Auth.DTO.Response;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Messenger.Domain.Aggregates.ResetPasswordTokens.Errors;
using Messenger.Domain.Aggregates.ResetPasswordTokens.ValueObjects;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Commands.ResetPassword
{
    internal sealed class ResetPasswordCommandHandler
        : ICommandHandler<ResetPasswordCommand>
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IResetPasswordTokenRepository _resetPasswordTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenHashService _tokenHashService;

        public ResetPasswordCommandHandler(
            IIdentityService<ApplicationUser> identityService,
            IResetPasswordTokenRepository resetPasswordTokenRepository,
            IUnitOfWork unitOfWork,
            ITokenHashService tokenHashService)
        {
            _identityService = identityService;
            _resetPasswordTokenRepository = resetPasswordTokenRepository;
            _unitOfWork = unitOfWork;
            _tokenHashService = tokenHashService;
        }


        public async Task<Result> Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            #region Validate request

            // Check if the user exists

            var userId = new UserId(request.UserId);

            var user = await _identityService.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    UserErrors.NotFound);
            }

            // Check if the token exists

            var tokenId = new ResetPasswordTokenId(request.TokenId);

            var repositoryToken = await _resetPasswordTokenRepository.GetByIdWithUserAsync(tokenId, cancellationToken);

            if (repositoryToken is null)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ResetPasswordTokenErrors.NotFound);
            }

            // Check if user has the token

            if (repositoryToken.User.Id != userId)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ResetPasswordTokenErrors.TokenNotAssignedToUser);
            }

            // Check if the token is already used

            if (repositoryToken.IsUsed)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ResetPasswordTokenErrors.AlreadyUsed);
            }

            // Check if the token is expired

            if (repositoryToken.ExpiresAt.Value < DateTime.UtcNow)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ResetPasswordTokenErrors.Expired);
            }

            #endregion

            // Unescape the token

            var unescapedToken = Uri.UnescapeDataString(request.Token);

            // Verify the token

            var tokenIsValid = _tokenHashService.Verify(unescapedToken, repositoryToken.TokenHash.Value);

            if (!tokenIsValid)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ResetPasswordTokenErrors.Invalid);
            }

            // Use the token

            repositoryToken.Use();

            // Update user password

            await _identityService.ResetPasswordAsync(
                user,
                request.NewPassword);

            // Save changes

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
