using Messenger.Application.Abstractions.Emails;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Identity;
using Messenger.Infrastructure.Extensions.DI.Shared;
using Messenger.Infrastructure.Services;
using Messenger.Infrastructure.Services.Emails;
using Messenger.Infrastructure.Services.Emails.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;

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

            #region Email services
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailLinkGenerator, EmailLinkGenerator>();
            services.AddScoped<IEmailLetterGenerator, EmailLetterGenerator>();
            services.AddFluentEmailSender();
            #endregion

            return services;
        }

        private static IServiceCollection AddFluentEmailSender(
            this IServiceCollection services)
        {
            var smtpClientSettings = services.BuildServiceProvider()
                .GetOptions<SmtpClientSettings>();

            SmtpClient client = new SmtpClient(smtpClientSettings.Host, smtpClientSettings.Port)
            {
                Credentials = new NetworkCredential(
                    smtpClientSettings.Username, 
                    smtpClientSettings.Password),
                EnableSsl = smtpClientSettings.EnableSsl
            };

            services
                .AddFluentEmail(smtpClientSettings.Username)
                .AddSmtpSender(client);

            return services;
        }
    }
}
