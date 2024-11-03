using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Users.DTO;
using Messenger.Application.Features.Users.Mappers;
using Messenger.Domain.Aggregates.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Application.Extensions.DI
{
    public static class MapperExtensions
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            #region Users
            services.AddMapper<User, UserResponse, UserToUserResponseMapper>();
            #endregion
            return services;
        }
        private static IServiceCollection AddMapper<TSource, TDestination, TMapper>(
            this IServiceCollection services)
            where TSource : class
            where TDestination : class
            where TMapper : Mapper<TSource, TDestination>
        {
            services.AddScoped<Mapper<TSource, TDestination>, TMapper>();
            return services;
        }
    }
}
