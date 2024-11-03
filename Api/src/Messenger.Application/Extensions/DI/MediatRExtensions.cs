using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Messenger.Application.Extensions.DI
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
