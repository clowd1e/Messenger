using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Extensions.DI
{
    public static class RazorExtensions
    {
        public static IServiceCollection AddAppRazorTemplating(this IServiceCollection services)
        {
            services.AddRazorTemplating();

            return services;
        }
    }
}
