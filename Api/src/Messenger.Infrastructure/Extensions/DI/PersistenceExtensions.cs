using Messenger.Application.Abstractions.Data;
using Messenger.Application.Identity;
using Messenger.Infrastructure.Extensions.DI.Shared;
using Messenger.Infrastructure.Persistence;
using Messenger.Infrastructure.Persistence.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Extensions.DI
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddMessengerDbContext();

            services.AddUnitOfWork();

            services.AddAppIdentity();

            return services;
        }

        private static IServiceCollection AddMessengerDbContext(this IServiceCollection services)
        {
            services.AddDbContext<MessengerDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServerWithSettings(serviceProvider);
            });

            return services;
        }

        private static DbContextOptionsBuilder UseSqlServerWithSettings(
            this DbContextOptionsBuilder options,
            IServiceProvider serviceProvider)
        {
            var dbSettings = serviceProvider.GetOptions<DbSettings>();

            return options.UseSqlServer(connectionString: dbSettings.ConnectionString);
        }

        private static IServiceCollection AddUnitOfWork(
            this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static IServiceCollection AddAppIdentity(
            this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Tokens.AuthenticatorTokenProvider = "Default";
            })
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<MessengerDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
