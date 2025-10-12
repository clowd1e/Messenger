using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Auth.DTO.RequestModel;
using Messenger.Application.Identity.Options;
using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Common.TokenHash;
using Messenger.Domain.Aggregates.RefreshTokens;
using Messenger.Domain.Aggregates.RefreshTokens.ValueObjects;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Features.Auth.Mappers
{
    internal sealed class CreateRefreshTokenRequestMapper
        : Mapper<CreateRefreshTokenRequestModel, Result<RefreshToken>>
    {
        private readonly RefreshTokenSettings _settings;

        public CreateRefreshTokenRequestMapper(
            IOptions<RefreshTokenSettings> settings)
        {
            _settings = settings.Value;
        }

        public override Result<RefreshToken> Map(CreateRefreshTokenRequestModel source)
        {
            // Generate token hash

            var tokenHashResult = TokenHash.Create(source.TokenHash);

            if (tokenHashResult.IsFailure)
            {
                return Result.Failure<RefreshToken>(tokenHashResult.Error);
            }

            var tokenHash = tokenHashResult.Value;

            // Generate a new sessionId

            var sessionId = new SessionId(source.SessionId);

            // Calculate expiration timestamp

            var expiresAtResult = Timestamp.Create(
                DateTime.UtcNow.Add(
                    TimeSpan.FromDays(_settings.ExpirationTimeInDays)
                    ));

            if (expiresAtResult.IsFailure)
            {
                return Result.Failure<RefreshToken>(expiresAtResult.Error);
            }

            var expiresAt = expiresAtResult.Value;

            // Create RefreshToken

            var tokenResult = RefreshToken.Create(
                sessionId: sessionId,
                tokenHash: tokenHash,
                expiresAt: expiresAt,
                user: source.User);

            if (tokenResult.IsFailure)
            {
                return Result.Failure<RefreshToken>(tokenResult.Error);
            }

            return tokenResult.Value;
        }
    }
}
