using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Auth.DTO.Response;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.ConfirmEmailTokens;
using Messenger.Domain.Aggregates.ConfirmEmailTokens.Errors;
using Messenger.Domain.Aggregates.ConfirmEmailTokens.ValueObjects;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Commands.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandHandler
        : ICommandHandler<ConfirmEmailCommand>
    {
        private readonly IConfirmEmailTokenRepository _confirmEmailTokenRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenHashService _tokenHashService;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmEmailCommandHandler(
            IConfirmEmailTokenRepository confirmEmailTokenRepository,
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            ITokenHashService tokenHashService,
            IUnitOfWork unitOfWork)
        {
            _confirmEmailTokenRepository = confirmEmailTokenRepository;
            _identityService = identityService;
            _userRepository = userRepository;
            _tokenHashService = tokenHashService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ConfirmEmailCommand request,
            CancellationToken cancellationToken)
        {
            #region Validate request

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

            var repositoryToken = await _confirmEmailTokenRepository.GetByIdWithUserAsync(tokenId, cancellationToken);

            if (repositoryToken is null)
            {
                return Result.Failure<ValidateEmailConfirmationResponse>(
                    ConfirmEmailTokenErrors.NotFound);
            }

            // Check if user has the token

            if (repositoryToken.User.Id != userId)
            {
                return Result.Failure<ValidateEmailConfirmationResponse>(
                    ConfirmEmailTokenErrors.TokenNotAssignedToUser);
            }

            // Check if the token is already used

            if (repositoryToken.IsUsed)
            {
                return Result.Failure<ValidateEmailConfirmationResponse>(
                    ConfirmEmailTokenErrors.AlreadyUsed);
            }

            // Check if the token is expired

            if (repositoryToken.ExpiresAt.Value < DateTime.UtcNow)
            {
                return Result.Failure<ValidateEmailConfirmationResponse>(
                    ConfirmEmailTokenErrors.Expired);
            }

            #endregion

            // Check if the user already has an email confirmed

            if (user.EmailConfirmed)
            {
                return UserErrors.EmailAlreadyConfirmed;
            }

            // Get identityUser

            var identityUser = await _identityService.GetByEmailAsync(user.Email);

            if (identityUser is null)
            {
                throw new DataInconsistencyException();
            }

            // Unescape the token

            var unescapedToken = Uri.UnescapeDataString(request.Token);

            // Verify the token

            var tokenIsValid = _tokenHashService.Verify(unescapedToken, repositoryToken.TokenHash.Value);

            if (!tokenIsValid)
            {
                return Result.Failure<ValidatePasswordRecoveryResponse>(
                    ConfirmEmailTokenErrors.Invalid);
            }

            // Confirm the email in the user aggregate

            var result = user.ConfirmEmail();

            if (result.IsFailure)
            {
                return result.Error;
            }

            // Use the token

            repositoryToken.Use();

            // Confirm the email in the identity service

            var identityResult = await _identityService.ConfirmEmailAsync(identityUser);

            if (identityResult.IsFailure)
            {
                return identityResult.Error;
            }

            // Save changes

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
