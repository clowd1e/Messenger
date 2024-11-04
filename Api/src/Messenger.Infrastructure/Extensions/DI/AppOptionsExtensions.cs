using Messenger.Infrastructure.Extensions.DI.Shared;
using Messenger.Infrastructure.Persistense.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Extensions.DI
{
    public static class AppOptionsExtensions
    {
        public static IServiceCollection AddAppOptions(
            this IServiceCollection services)
        {
            services.ConfigureValidatableOnStartOptions<DbSettings>(
                DbSettings.SectionName);

            return services;
        }
    }
}
