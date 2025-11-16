using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Auth.DTO.RequestModel;
using Messenger.Application.Identity.Options;
using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Common.TokenHash;
using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Messenger.Domain.Aggregates.ResetPasswordTokens.ValueObjects;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Features.Auth.Mappers
{
    internal sealed class CreateResetPasswordTokenRequestMapper
        : Mapper<CreateResetPasswordTokenRequestModel, Result<ResetPasswordToken>>
    {
        private readonly ResetPasswordTokenSettings _settings;

        public CreateResetPasswordTokenRequestMapper(
            IOptions<ResetPasswordTokenSettings> settings)
        {
            _settings = settings.Value;
        }

        public override Result<ResetPasswordToken> Map(
            CreateResetPasswordTokenRequestModel source)
        {
            // Generate token hash

            var tokenHashResult = TokenHash.Create(source.TokenHash);

            if (tokenHashResult.IsFailure)
            {
                return Result.Failure<ResetPasswordToken>(tokenHashResult.Error);
            }

            var tokenHash = tokenHashResult.Value;

            // Generate a new ResetPasswordTokenId

            var tokenId = new ResetPasswordTokenId(Guid.NewGuid());

            // Calculate expiration timestamp

            var expiresAtResult = Timestamp.Create(
                DateTime.UtcNow.Add(
                    TimeSpan.FromHours(_settings.ExpirationTimeInHours)
                    ));

            if (expiresAtResult.IsFailure)
            {
                return Result.Failure<ResetPasswordToken>(expiresAtResult.Error);
            }

            var expiresAt = expiresAtResult.Value;

            // Create ResetPasswordToken

            var tokenResult = ResetPasswordToken.Create(
                resetPasswordTokenId: tokenId,
                tokenHash: tokenHash,
                expiresAt: expiresAt,
                user: source.User);

            if (tokenResult.IsFailure)
            {
                return Result.Failure<ResetPasswordToken>(tokenResult.Error);
            }

            return tokenResult.Value;
        }
    }
}
