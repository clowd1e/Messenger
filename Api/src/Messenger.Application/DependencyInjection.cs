using Messenger.Application.Extensions.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddApplicationServices();

            services.AddMappers();

            services.AddMediatR();

            services.AddPipelineBehaviors();

            services.AddValidators();

            return services;
        }
    }
}
