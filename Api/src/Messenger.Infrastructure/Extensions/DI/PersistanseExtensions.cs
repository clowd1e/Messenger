using Messenger.Application.Abstractions.Data;
using Messenger.Infrastructure.Extensions.DI.Shared;
using Messenger.Infrastructure.Persistense;
using Messenger.Infrastructure.Persistense.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Extensions.DI
{
    public static class PersistanseExtensions
    {
        public static IServiceCollection AddPersistense(this IServiceCollection services)
        {
            services.AddMessengerDbContext();

            services.AddUnitOfWork();

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
    }
}
