using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Common.TokenHash;
using Messenger.Domain.Aggregates.ConfirmEmailTokens.Errors;
using Messenger.Domain.Aggregates.ConfirmEmailTokens.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.ConfirmEmailTokens
{
    public sealed class ConfirmEmailToken
        : AggregateRoot<ConfirmEmailTokenId>
    {
        private TokenHash _tokenHash;
        private Timestamp _expiresAt;

        private ConfirmEmailToken()
            : base(new(Guid.NewGuid())) { }

        public ConfirmEmailToken(
            ConfirmEmailTokenId confirmEmailTokenId,
            TokenHash tokenHash,
            Timestamp expiresAt,
            Users.User user) : base(confirmEmailTokenId)
        {
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            User = user;
        }

        public TokenHash TokenHash
        {
            get => _tokenHash;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);
                _tokenHash = value;
            }
        }

        public Timestamp ExpiresAt
        {
            get => _expiresAt;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);
                _expiresAt = value;
            }
        }

        public bool IsActive => !IsUsed && ExpiresAt.Value < DateTime.UtcNow;

        public bool IsUsed { get; private set; }

        public Users.User? User { get; private set; } = default;

        public Result Use()
        {
            if (IsUsed)
            {
                return Result.Failure(
                    ConfirmEmailTokenErrors.AlreadyUsed);
            }

            IsUsed = true;

            return Result.Success();
        }

        public static Result<ConfirmEmailToken> Create(
            ConfirmEmailTokenId confirmEmailTokenId,
            TokenHash tokenHash,
            Timestamp expiresAt,
            Users.User user)
        {
            return new ConfirmEmailToken(
                confirmEmailTokenId,
                tokenHash,
                expiresAt,
                user);
        }
    }
}
