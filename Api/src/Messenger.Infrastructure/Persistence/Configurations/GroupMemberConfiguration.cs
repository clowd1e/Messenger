using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.GroupChats;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistence.Configurations
{
    internal sealed class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        private const string UserId = "user_id";
        private const string ChatId = "chat_id";

        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder.ToTable("group_member");

            // Define shadow properties for composite primary key

            builder.Property<UserId>(UserId)
                .HasConversion(
                    userId => userId.Value,
                    value => new UserId(value));

            builder.Property<ChatId>(ChatId)
                .HasConversion(
                    chatId => chatId.Value,
                    value => new ChatId(value));

            builder.HasKey(UserId, ChatId);

            builder.Property(member => member.Role)
                .HasColumnName("role");

            builder
                .HasOne(member => member.User)
                .WithMany(user => user.GroupMembers)
                .HasForeignKey(UserId)
                .IsRequired();

            builder
                .HasOne(member => member.GroupChat)
                .WithMany(chat => chat.GroupMembers)
                .HasForeignKey(ChatId)
                .IsRequired();
        }
    }
}
