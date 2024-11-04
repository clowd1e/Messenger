using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Extensions.DI.Shared
{
    public static class SharedExtensions
    {
        public static IServiceCollection ConfigureValidatableOnStartOptions<TOptions>(
           this IServiceCollection services,
           string configSectionPath)
           where TOptions : class, new()
        {
            services
                .AddOptionsWithValidateOnStart<TOptions>(configSectionPath)
                .ValidateDataAnnotations();

            return services;
        }

        public static TOptions GetOptions<TOptions>(this IServiceProvider serviceProvider)
            where TOptions : class
        {
            return serviceProvider.GetRequiredService<IOptions<TOptions>>().Value;
        }
    }
}
