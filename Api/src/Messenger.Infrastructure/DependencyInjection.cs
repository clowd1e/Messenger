using Messenger.Infrastructure.Extensions.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddAppOptions();

            services.AddInfrastructureServices();

            services.AddExternalServices();

            services.AddPersistense();

            services.AddRepositories();

            return services;
        }
    }
}
