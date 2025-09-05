using Messenger.Domain.Aggregates.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistence.Configurations
{
    internal sealed class PrivateChatConfiguration : IEntityTypeConfiguration<PrivateChat>
    {
        public void Configure(EntityTypeBuilder<PrivateChat> builder)
        {
            builder.ToTable("private_chat");
        }
    }
}
