using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Common.TokenHash;
using Messenger.Domain.Aggregates.RefreshTokens;
using Messenger.Domain.Aggregates.RefreshTokens.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistence.Configurations
{
    internal sealed class RefreshTokenConfiguration
        : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_token");

            builder.HasKey(token => new { token.UserId, token.SessionId });

            builder.Property(token => token.UserId)
                .HasConversion(
                    userId => userId.Value,
                    value => new UserId(value));

            builder.Property(token => token.SessionId)
                .HasConversion(
                    sessionId => sessionId.Value,
                    value => new SessionId(value));

            builder.Property(token => token.TokenHash)
                .HasMaxLength(TokenHash.MaxLength)
                .HasConversion(
                    tokenHash => tokenHash.Value,
                    value => TokenHash.Create(value).Value);

            builder.Property(token => token.ExpiresAt)
                .HasConversion(
                    timestamp => timestamp.Value,
                    value => Timestamp.Create(value.ToUniversalTime()).Value);

            builder
                .HasOne(token => token.User)
                .WithMany(user => user.RefreshTokens)
                .HasForeignKey(token => token.UserId)
                .IsRequired();
        }
    }
}
