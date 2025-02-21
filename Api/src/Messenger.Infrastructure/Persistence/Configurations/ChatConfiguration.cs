﻿using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users;
using Messenger.Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistence.Configurations
{
    internal sealed class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("chat");

            builder.HasIndex(chat => chat.Id)
                .IsUnique();

            builder.Property(chat => chat.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ChatId(value))
                .HasColumnName("id");

            builder.Property(chat => chat.CreationDate)
                .HasConversion(
                    date => date.Value,
                    value => ChatCreationDate.Create(value.ToUniversalTime()).Value)
                .HasColumnName("creation_date");

            builder
                .HasMany(chat => chat.Messages)
                .WithOne(message => message.Chat);

            builder
                .HasMany(chat => chat.Users)
                .WithMany(user => user.Chats)
                .UsingEntity(
                    ManyToManyTables.UserChat,
                    l => l.HasOne(typeof(Chat)).WithMany().HasForeignKey("chats_id"),
                    r => r.HasOne(typeof(User)).WithMany().HasForeignKey("users_id"));
        }
    }
}
