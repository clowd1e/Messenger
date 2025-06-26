using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Auth.DTO.Response;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Commands.Login
{
    internal sealed class LoginCommandHandler
        : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _identityService = identityService;
            _userRepository = userRepository;
            _tokenService = tokenService;
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

            var identityUser = await _identityService.GetByEmailAsync(email);

            if (identityUser is null)
            {
                return Result.Failure<LoginResponse>(UserErrors.NotFound);
            }

            var loginResult = await _identityService.LoginAsync(
                identityUser, request.Password);

            if (loginResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(loginResult.Error);
            }

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                throw new DataInconsistencyException();
            }

            if (!user.EmailConfirmed)
            {
                return Result.Failure<LoginResponse>(UserErrors.EmailNotConfirmed);
            }

            var refreshToken = _tokenService.GenerateRefreshToken();

            await _identityService.PopulateRefreshTokenAsync(identityUser, refreshToken);

            var accessToken = _tokenService.GenerateAccessToken(user);

            return new LoginResponse(accessToken, refreshToken);
        }
    }
}
