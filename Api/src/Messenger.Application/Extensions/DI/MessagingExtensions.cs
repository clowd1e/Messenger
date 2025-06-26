using Messenger.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Messenger.Application.Extensions.DI
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(
            this IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromAssembliesOf(typeof(MessagingExtensions))
                    .AddHandlersOfType(typeof(IQueryHandler<,>))
                    .AddHandlersOfType(typeof(ICommandHandler<>))
                    .AddHandlersOfType(typeof(ICommandHandler<,>)));

            return services;
        }

        private static IImplementationTypeSelector AddHandlersOfType(
            this IImplementationTypeSelector selector,
            Type assignableType)
        {
            return selector.AddClasses(classes => classes.AssignableTo(assignableType), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        }
    }
}
