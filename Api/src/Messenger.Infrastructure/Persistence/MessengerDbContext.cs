using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.ConfirmEmailTokens;
using Messenger.Domain.Aggregates.GroupChats;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.RefreshTokens;
using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Messenger.Domain.Aggregates.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence
{
    public sealed class MessengerDbContext : IdentityDbContext<ApplicationUser>
    {
        public MessengerDbContext(
            DbContextOptions<MessengerDbContext> options)
            : base(options) { }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<PrivateChat> PrivateChats { get; set; }

        public DbSet<GroupChat> GroupChats { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public new DbSet<User> Users { get; set; }

        public DbSet<ConfirmEmailToken> ConfirmEmailTokens { get; set; }

        public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessengerDbContext).Assembly);
        }
    }
}
