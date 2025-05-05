using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Emails;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Features.Auth.DTO;
using Messenger.Application.Identity.Options;
using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Messenger.Domain.Aggregates.ResetPasswordTokens.Errors;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Features.Auth.Commands.RequestPasswordRecovery
{
    internal sealed class RequestPasswordRecoveryCommandHandler
        : IRequestHandler<RequestPasswordRecoveryCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ITokenHashService _tokenHashService;
        private readonly IResetPasswordTokenRepository _resetPasswordTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly Mapper<RequestPasswordRecoveryCommand, Result<Email>> _commandMapper;
        private readonly Mapper<CreateResetPasswordTokenRequestModel, Result<ResetPasswordToken>> _tokenMapper;
        private readonly ResetPasswordTokenSettings _resetPasswordTokenSettings;

        public RequestPasswordRecoveryCommandHandler(
            IUserRepository userRepository,
            ITokenService tokenService,
            ITokenHashService tokenHashService,
            IResetPasswordTokenRepository resetPasswordTokenRepository,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            Mapper<RequestPasswordRecoveryCommand, Result<Email>> commandMapper,
            Mapper<CreateResetPasswordTokenRequestModel, Result<ResetPasswordToken>> tokenMapper,
            IOptions<ResetPasswordTokenSettings> resetPasswordTokenSettings)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _tokenHashService = tokenHashService;
            _resetPasswordTokenRepository = resetPasswordTokenRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _commandMapper = commandMapper;
            _tokenMapper = tokenMapper;
            _resetPasswordTokenSettings = resetPasswordTokenSettings.Value;
        }

        public async Task<Result> Handle(
            RequestPasswordRecoveryCommand request,
            CancellationToken cancellationToken)
        {
            // Map request to email

            var commandMappingResult = _commandMapper.Map(request);

            if (commandMappingResult.IsFailure)
            {
                return commandMappingResult.Error;
            }

            var email = commandMappingResult.Value;

            // Check if user exists

            var user = await _userRepository
                .GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            // Check how many user has active reset password tokens

            var activeTokensCount = await _resetPasswordTokenRepository
                .CountActiveTokensAsync(user.Id, cancellationToken);

            if (activeTokensCount > _resetPasswordTokenSettings.ActiveTokensLimit)
            {
                return ResetPasswordTokenErrors.TooManyActiveTokens;
            }

            // Generate reset password token

            var token = _tokenService.GenerateResetPasswordToken();

            var tokenHash = _tokenHashService.Hash(token);

            var requestPasswordTokenModel = new CreateResetPasswordTokenRequestModel(
                tokenHash,
                user);

            var tokenMappingResult = _tokenMapper.Map(requestPasswordTokenModel);

            if (tokenMappingResult.IsFailure)
            {
                return tokenMappingResult.Error;
            }

            var resetPasswordToken = tokenMappingResult.Value;

            // Insert reset password token into repository

            await _resetPasswordTokenRepository
                .InsertAsync(resetPasswordToken, cancellationToken);

            // Save changes to the database

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send email with reset password token

            await _emailService
                .SendPasswordRecoveryEmailAsync(
                    email.Value,
                    user.Id.Value.ToString(),
                    resetPasswordToken.Id.Value.ToString(),
                    token);

            // Return success result

            return Result.Success();
        }
    }
}
