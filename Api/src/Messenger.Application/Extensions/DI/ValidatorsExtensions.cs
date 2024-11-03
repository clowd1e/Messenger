using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Messenger.Application.Extensions.DI
{
    public static class ValidatorsExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly(),
                includeInternalTypes: true);

            return services;
        }
    }
}
