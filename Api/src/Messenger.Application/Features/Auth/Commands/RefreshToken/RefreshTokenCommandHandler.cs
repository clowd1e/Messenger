using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Auth.DTO.Response;
using Messenger.Application.Identity.Options;
using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Common.TokenHash;
using Messenger.Domain.Aggregates.RefreshTokens;
using Messenger.Domain.Aggregates.RefreshTokens.Errors;
using Messenger.Domain.Aggregates.RefreshTokens.ValueObjects;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.Extensions.Options;
using RefreshTokens = Messenger.Domain.Aggregates.RefreshTokens;

namespace Messenger.Application.Features.Auth.Commands.RefreshToken
{
    internal sealed class RefreshTokenCommandHandler
        : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenHashService _tokenHashService;
        private readonly IUserRepository _userRepository;
        private readonly RefreshTokenSettings _refreshTokenSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            ITokenService tokenService,
            ITokenHashService tokenHashService,
            IUserRepository userRepository,
            IOptions<RefreshTokenSettings> refreshTokenSettings,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _tokenHashService = tokenHashService;
            _userRepository = userRepository;
            _refreshTokenSettings = refreshTokenSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(
           RefreshTokenCommand request,
           CancellationToken cancellationToken)
        {
            // Validate the access token

            var accessValidationResult = await _tokenService.ValidateAccessTokenAsync(
                request.AccessToken, false);

            if (accessValidationResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(accessValidationResult.Error);
            }

            var userId = new UserId(_tokenService.GetUserIdOutOfAccessToken(request.AccessToken));

            // Get refresh token by session ID

            var sessionId = new SessionId(request.SessionId);

            var refreshToken = await _refreshTokenRepository.GetBySessionIdWithUserAsync(
                sessionId,
                userId,
                cancellationToken);

            if (refreshToken is null)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.NotFound);
            }

            // Get domain user

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<RefreshTokenResponse>(UserErrors.NotFound);
            }

            // Validate refresh token expiration time

            if (refreshToken.ExpiresAt.Value <= DateTime.UtcNow)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.TokenExpired);
            }

            // Validate the refresh token hash

            var TokenHashisValid = _tokenHashService.Verify(
                request.RefreshToken,
                refreshToken.TokenHash.Value);

            if (!TokenHashisValid)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.InvalidToken);
            }

            // Populate the refresh token

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var newRefreshTokenHash = _tokenHashService.Hash(newRefreshToken);

            var populateRefreshTokenResult = PopulateRefreshToken(
                refreshToken,
                newRefreshTokenHash);

            if (populateRefreshTokenResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(populateRefreshTokenResult.Error);
            }

            // Generate new access token

            var accessToken = _tokenService.GenerateAccessToken(user);

            // Save the updated refresh token

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(
                new RefreshTokenResponse(accessToken, newRefreshToken));
        }

        private Result PopulateRefreshToken(
            RefreshTokens.RefreshToken domainRefreshToken,
            string newTokenHash)
        {
            var tokenHashResult = TokenHash.Create(newTokenHash);

            if (tokenHashResult.IsFailure)
            {
                return tokenHashResult.Error;
            }

            var tokenHash = tokenHashResult.Value;

            var expiresAtResult = Timestamp.Create(
                DateTime.UtcNow.Add(
                    TimeSpan.FromDays(
                        _refreshTokenSettings.ExpirationTimeInDays)));

            if (expiresAtResult.IsFailure)
            {
                return expiresAtResult.Error;
            }

            var expiresAt = expiresAtResult.Value;

            domainRefreshToken.Populate(tokenHash, expiresAt);

            return Result.Success();
        }
    }
}
