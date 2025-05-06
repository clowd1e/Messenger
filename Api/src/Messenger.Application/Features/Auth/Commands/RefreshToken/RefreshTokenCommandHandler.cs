using MediatR;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Auth.DTO.Response;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Commands.RefreshToken
{
    internal sealed class RefreshTokenCommandHandler
        : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _identityService = identityService;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var accessTokenValidationResult = await _tokenService.ValidateAccessTokenAsync(
                request.AccessToken, validateLifetime: false);

            if (accessTokenValidationResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(accessTokenValidationResult.Error);
            }

            var appUserResult = await _identityService.GetByRefreshTokenAsync(
                request.RefreshToken);

            if (appUserResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(appUserResult.Error);
            }

            var appUser = appUserResult.Value;

            var refreshTokenValidationResult = _identityService.ValidateRefreshToken(appUser);

            if (refreshTokenValidationResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(refreshTokenValidationResult.Error);
            }

            var user = await _userRepository.GetByEmailAsync(
                Email.Create(appUser.Email).Value);

            if (user is null)
            {
                throw new DataInconsistencyException();
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            await _identityService.PopulateRefreshTokenAsync(appUser, newRefreshToken);

            return new RefreshTokenResponse(accessToken, newRefreshToken);
        }
    }
}
