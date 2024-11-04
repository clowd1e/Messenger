using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Messages.ValueObjects;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Infrastructure.Persistense.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistense.Configurations
{
    internal sealed class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasIndex(chat => chat.Id)
                .IsUnique();

            builder.Property(chat => chat.Id).HasConversion(
                id => id.Value,
                value => new ChatId(value));

            builder.Property(chat => chat.CreationDate)
                .HasConversion(
                    date => date.Value,
                    value => ChatCreationDate.Create(value).Value);

            builder.OwnsMany(chat => chat.Messages, messagesBuilder =>
            {
                messagesBuilder.Property(message => message.UserId)
                    .HasConversion(
                        userId => userId.Value,
                        value => new UserId(value));

                messagesBuilder.Property(message => message.Content)
                    .HasMaxLength(MessageContent.MaxLength)
                    .HasConversion(
                        content => content.Value,
                        value => MessageContent.Create(value).Value);

                messagesBuilder.Property(message => message.Timestamp)
                    .HasConversion(
                        timestamp => timestamp.Value,
                        value => MessageTimestamp.Create(value).Value);
            });

            builder
                .HasMany(chat => chat.Users)
                .WithMany(user => user.Chats)
                .UsingEntity(join => join.ToTable(ManyToManyTables.UsersChats));
        }
    }
}
