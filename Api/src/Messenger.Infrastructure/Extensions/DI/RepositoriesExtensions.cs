﻿using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.ConfirmEmailTokens;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Messenger.Domain.Aggregates.Users;
using Messenger.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Extensions.DI
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IConfirmEmailTokenRepository, ConfirmEmailTokenRepository>();
            services.AddScoped<IResetPasswordTokenRepository, ResetPasswordTokenRepository>();

            return services;
        }
    }
}
