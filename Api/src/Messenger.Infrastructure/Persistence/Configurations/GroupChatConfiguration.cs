using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.GroupChats.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistence.Configurations
{
    internal sealed class GroupChatConfiguration : IEntityTypeConfiguration<GroupChat>
    {
        public void Configure(EntityTypeBuilder<GroupChat> builder)
        {
            builder.ToTable("group_chat");

            builder.Property(gc => gc.Name)
                .IsRequired()
                .HasMaxLength(GroupChatName.MaxLength)
                .HasConversion(
                    name => name.Value, 
                    value => GroupChatName.Create(value).Value)
                .HasColumnName("name");

            builder.Property(gc => gc.Description)
                .IsRequired(false)
                .HasMaxLength(GroupChatDescription.MaxLength)
                .HasConversion(
                    description => description!.Value, 
                    value => GroupChatDescription.Create(value).Value)
                .HasColumnName("description");

            builder
                .HasMany(chat => chat.GroupMembers)
                .WithOne(gm => gm.GroupChat);
        }
    }
}
