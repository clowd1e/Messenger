using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistense
{
    public sealed class MessengerDbContext : IdentityDbContext<ApplicationUser>
    {
        public MessengerDbContext(
            DbContextOptions<MessengerDbContext> options)
            : base(options) { }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessengerDbContext).Assembly);
        }
    }
}
