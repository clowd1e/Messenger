using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Application.Extensions.DI
{
    public static class PipelineBehaviorsExtensions
    {
        public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services)
        {
            services.TryDecorate(typeof(IQueryHandler<,>), typeof(ValidationDecorator.QueryHandler<,>));
            services.TryDecorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandHandler<>));
            services.TryDecorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));

            return services;
        }
    }
}
