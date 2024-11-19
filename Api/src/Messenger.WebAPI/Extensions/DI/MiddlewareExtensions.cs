using Messenger.WebAPI.Middleware;

namespace Messenger.WebAPI.Extensions.DI
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder AddMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionHandlingMiddleware>();

            return builder;
        }
    }
}
