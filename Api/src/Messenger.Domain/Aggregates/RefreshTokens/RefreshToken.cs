using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Common.TokenHash;
using Messenger.Domain.Aggregates.RefreshTokens.Errors;
using Messenger.Domain.Aggregates.RefreshTokens.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.RefreshTokens
{
    public sealed class RefreshToken
    {
        private TokenHash _tokenHash;
        private Timestamp _expiresAt;

        private RefreshToken() { }

        private RefreshToken(
            SessionId sessionId,
            TokenHash tokenHash,
            Timestamp expiresAt,
            Users.User user)
        {
            SessionId = sessionId;
            _tokenHash = tokenHash;
            _expiresAt = expiresAt;
            User = user;
        }

        public UserId UserId { get; private set; }

        public SessionId SessionId { get; private set; }

        public TokenHash TokenHash
        {
            get => _tokenHash;
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));
                _tokenHash = value;
            }
        }

        public Timestamp ExpiresAt
        {
            get => _expiresAt;
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));
                _expiresAt = value;
            }
        }

        public Users.User? User { get; private set; } = default;

        public Result Populate(
            TokenHash tokenHash,
            Timestamp expiresAt)
        {
            if (expiresAt.Value <= DateTime.UtcNow)
            {
                return Result.Failure(RefreshTokenErrors.InvalidExpiryTime);
            }

            _tokenHash = tokenHash;
            _expiresAt = expiresAt;

            return Result.Success();
        }

        public static Result<RefreshToken> Create(
            SessionId sessionId,
            TokenHash tokenHash,
            Timestamp expiresAt,
            Users.User user)
        {
            return new RefreshToken(
                sessionId,
                tokenHash,
                expiresAt,
                user);
        }
    }
}
