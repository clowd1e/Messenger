using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using MessengerDbContext dbContext = scope.ServiceProvider.GetRequiredService<MessengerDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
