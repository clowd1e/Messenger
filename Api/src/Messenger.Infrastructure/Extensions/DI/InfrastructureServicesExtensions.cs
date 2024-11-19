using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Identity;
using Messenger.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Extensions.DI
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<ApplicationUser>, IdentityPasswordHasher>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IIdentityService<ApplicationUser>, IdentityService>();
            services.AddScoped<ITokenHashService, TokenHashService>();
            services.AddScoped<IUserContextService<Guid>, UserContextService>();

            return services;
        }
    }
}
