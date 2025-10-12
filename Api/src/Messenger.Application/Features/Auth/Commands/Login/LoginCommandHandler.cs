using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Auth.DTO.RequestModel;
using Messenger.Application.Features.Auth.DTO.Response;
using Messenger.Application.Identity;
using Messenger.Application.Identity.Options;
using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Common.TokenHash;
using Messenger.Domain.Aggregates.RefreshTokens;
using Messenger.Domain.Aggregates.RefreshTokens.ValueObjects;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.Extensions.Options;
using RefreshTokens = Messenger.Domain.Aggregates.RefreshTokens;

namespace Messenger.Application.Features.Auth.Commands.Login
{
    internal sealed class LoginCommandHandler
        : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenHashService _tokenHashService;
        private readonly ITokenService _tokenService;
        private readonly LoginSettings _loginSettings;
        private readonly RefreshTokenSettings _refreshTokenSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper<CreateRefreshTokenRequestModel, Result<RefreshTokens.RefreshToken>> _refreshTokenMapper;

        public LoginCommandHandler(
            IIdentityService<ApplicationUser> identityService,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            ITokenHashService tokenHashService,
            ITokenService tokenService,
            IOptions<LoginSettings> loginSettings,
            IOptions<RefreshTokenSettings> refreshTokenSettings,
            IUnitOfWork unitOfWork,
            Mapper<CreateRefreshTokenRequestModel, Result<RefreshTokens.RefreshToken>> refreshTokenMapper)
        {
            _identityService = identityService;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _tokenHashService = tokenHashService;
            _tokenService = tokenService;
            _loginSettings = loginSettings.Value;
            _refreshTokenSettings = refreshTokenSettings.Value;
            _unitOfWork = unitOfWork;
            _refreshTokenMapper = refreshTokenMapper;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(emailResult.Error);
            }

            var email = emailResult.Value;

            // Login with provided credentials

            var identityUser = await _identityService.GetByEmailAsync(email);

            if (identityUser is null)
            {
                return Result.Failure<LoginResponse>(UserErrors.NotFound);
            }

            var loginResult = await _identityService.LoginAsync(
                identityUser,
                password: request.Password);

            if (loginResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(loginResult.Error);
            }

            // Get Domain User

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                throw new DataInconsistencyException();
            }

            if (!user.EmailConfirmed)
            {
                return Result.Failure<LoginResponse>(UserErrors.EmailNotConfirmed);
            }

            // Generate access and refresh tokens

            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenHash = _tokenHashService.Hash(refreshToken);

            var accessToken = _tokenService.GenerateAccessToken(user);

            // Check if refresh token already exists for the session

            var sessionId = new SessionId(request.SessionId);

            var existingRefreshToken = await _refreshTokenRepository.GetBySessionIdWithUserAsync(
                sessionId,
                user.Id,
                cancellationToken);

            Result? result;

            if (existingRefreshToken is not null)
            {
                result = PopulateRefreshToken(existingRefreshToken, refreshTokenHash);
            }
            else
            {
                result = await InsertRefreshToken(
                    user.Id,
                    request.SessionId,
                    user,
                    refreshTokenHash,
                    cancellationToken);
            }

            if (result.IsFailure)
            {
                return Result.Failure<LoginResponse>(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResponse(
                accessToken,
                refreshToken);
        }

        private async Task<Result> InsertRefreshToken(
            UserId userId,
            string sessionId,
            User user,
            string refreshTokenHash,
            CancellationToken cancellationToken)
        {

            // Check if user exceeds the maximum number of sessions

            var userSessionsCount = await _refreshTokenRepository.GetUserSessionsCountAsync(
                userId,
                cancellationToken);

            if (userSessionsCount >= _loginSettings.MaxSessionsCount)
            {
                return Result.Failure<LoginResponse>(UserErrors.MaxSessionsExceeded);
            }

            // Insert new refresh token

            var createRefreshTokenRequestModel = new CreateRefreshTokenRequestModel(
                refreshTokenHash,
                user,
                sessionId);

            var tokenMappingResult = _refreshTokenMapper.Map(createRefreshTokenRequestModel);

            if (tokenMappingResult.IsFailure)
            {
                return tokenMappingResult.Error;
            }

            var domainRefreshToken = tokenMappingResult.Value;

            await _refreshTokenRepository.InsertAsync(
                domainRefreshToken,
                cancellationToken);

            return Result.Success();
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
