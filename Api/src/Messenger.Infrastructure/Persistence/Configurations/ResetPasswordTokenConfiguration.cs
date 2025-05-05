using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Common.TokenHash;
using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Messenger.Domain.Aggregates.ResetPasswordTokens.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistence.Configurations
{
    internal sealed class ResetPasswordTokenConfiguration
        : IEntityTypeConfiguration<ResetPasswordToken>
    {
        public void Configure(EntityTypeBuilder<ResetPasswordToken> builder)
        {
            builder.ToTable("reset_password_token");

            builder.HasIndex(token => token.Id)
                .IsUnique();

            builder.Property(token => token.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ResetPasswordTokenId(value))
                .HasColumnName("id");

            builder.Property(token => token.TokenHash)
                .HasMaxLength(TokenHash.MaxLength)
                .HasConversion(
                    tokenHash => tokenHash.Value,
                    value => TokenHash.Create(value).Value)
                .HasColumnName("token_hash");

            builder.Property(token => token.ExpiresAt)
                .HasConversion(
                    timestamp => timestamp.Value,
                    value => Timestamp.Create(value.ToUniversalTime()).Value)
                .HasColumnName("expires_at");

            builder.Property(token => token.IsUsed)
                .HasColumnName("is_used");

            builder
                .HasOne(token => token.User)
                .WithMany(user => user.ResetPasswordTokens)
                .HasForeignKey("user_id")
                .IsRequired();
        }
    }
}
