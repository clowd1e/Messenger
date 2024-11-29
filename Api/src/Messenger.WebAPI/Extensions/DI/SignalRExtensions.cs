using Messenger.WebAPI.Hubs;

namespace Messenger.WebAPI.Extensions.DI
{
    public static class SignalRExtensions
    {
        public static IServiceCollection ConfigureSignalR(this IServiceCollection services)
        {
            services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(10);
            });

            return services;
        }

        public static IEndpointRouteBuilder MapSignalRHubs(this IEndpointRouteBuilder app)
        {
            app.MapHub<ChatHub>("/hubs/chat");

            return app;
        }
    }
}
