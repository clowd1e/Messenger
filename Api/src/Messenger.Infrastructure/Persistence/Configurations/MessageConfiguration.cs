using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Messages.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistence.Configurations
{
    internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("message");

            builder.HasIndex(message => message.Id)
               .IsUnique();

            builder.Property(message => message.Id)
                .HasConversion(
                    id => id.Value,
                    value => new MessageId(value))
                .HasColumnName("id");

            builder.Property(message => message.Content)
                .HasMaxLength(MessageContent.MaxLength)
                .HasConversion(
                    content => content.Value,
                    value => MessageContent.Create(value).Value)
                .HasColumnName("content");

            builder.Property(message => message.Timestamp)
                .HasConversion(
                    timestamp => timestamp.Value,
                    value => Timestamp.Create(value.ToUniversalTime()).Value)
                .HasColumnName("timestamp");

            builder
                .HasOne(message => message.Chat)
                .WithMany(chat => chat.Messages)
                .HasForeignKey("chat_id")
                .IsRequired();

            builder
                .HasOne(message => message.User)
                .WithMany(user => user.Messages)
                .HasForeignKey("user_id")
                .IsRequired();
        }
    }
}
