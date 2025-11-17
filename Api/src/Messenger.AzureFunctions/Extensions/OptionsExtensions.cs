using Messenger.AzureFunctions.Settings;
using Messenger.Infrastructure.Extensions.DI.Shared;
using Messenger.Infrastructure.Persistence.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.AzureFunctions.Extensions
{
    internal static class OptionsExtensions
    {
        public static IServiceCollection AddFunctionsOptions(this IServiceCollection services)
        {
            services.ConfigureValidatableOnStartOptions<DbSettings>(
                DbSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<RemoveUsersWithUnconfirmedEmailsSettings>(
                RemoveUsersWithUnconfirmedEmailsSettings.SectionName);

            return services;
        }
    }
}
