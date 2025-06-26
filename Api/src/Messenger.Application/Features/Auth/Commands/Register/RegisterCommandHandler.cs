using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Emails;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Auth.DTO.RequestModel;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.ConfirmEmailTokens;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Auth.Commands.Register
{
    internal sealed class RegisterCommandHandler
        : ICommandHandler<RegisterCommand>
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<RegisterCommand, Result<User>> _commandMapper;
        private readonly Mapper<User, ApplicationUser> _userMapper;
        private readonly Mapper<CreateConfirmEmailTokenRequestModel, Result<ConfirmEmailToken>> _tokenMapper;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly ITokenHashService _tokenHashService;
        private readonly IConfirmEmailTokenRepository _confirmEmailTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            Mapper<RegisterCommand, Result<User>> commandMapper,
            Mapper<User, ApplicationUser> userMapper,
            Mapper<CreateConfirmEmailTokenRequestModel, Result<ConfirmEmailToken>> tokenMapper,
            IEmailService emailService,
            ITokenService tokenService,
            ITokenHashService tokenHashService,
            IConfirmEmailTokenRepository confirmEmailTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _userRepository = userRepository;
            _commandMapper = commandMapper;
            _userMapper = userMapper;
            _tokenMapper = tokenMapper;
            _emailService = emailService;
            _tokenService = tokenService;
            _tokenHashService = tokenHashService;
            _confirmEmailTokenRepository = confirmEmailTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            // Map request to user

            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var user = mappingResult.Value;

            // Check if user is already registered

            var userExists = await _userRepository.ExistsAsync(
                user.Username,
                user.Email,
                cancellationToken);

            if (userExists)
            {
                return UserErrors.UserWithSameCredentialsAlreadyExists;
            }

            // Generate confirm email token

            var token = _tokenService.GenerateEmailConfirmationToken();

            var tokenHash = _tokenHashService.Hash(token);

            var confirmEmailTokenModel = new CreateConfirmEmailTokenRequestModel(
                tokenHash,
                user);

            var tokenMappingResult = _tokenMapper.Map(confirmEmailTokenModel);

            if (tokenMappingResult.IsFailure)
            {
                return tokenMappingResult.Error;
            }

            var confirmEmailToken = tokenMappingResult.Value;

            await _confirmEmailTokenRepository.InsertAsync(
                confirmEmailToken,
                cancellationToken);

            // Map user to identity user

            var identityUser = _userMapper.Map(user);

            // Insert user into the database

            await _userRepository.InsertAsync(user, cancellationToken);

            await _identityService.CreateAsync(identityUser, request.Password);

            // Save changes to the database

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send confirmation email

            await _emailService
                .SendConfirmationEmailAsync(
                    request.Email,
                    user.Id.Value.ToString(),
                    confirmEmailToken.Id.Value.ToString(),
                    token);

            return Result.Success();
        }
    }
}
