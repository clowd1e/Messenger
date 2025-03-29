using Messenger.Application.Abstractions.Emails;
using Messenger.Application.Abstractions.Storage;
using Messenger.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Application.Extensions.DI
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IImageService, ImageService>();

            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
