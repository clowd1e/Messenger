using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Auth.Commands.Register;
using Messenger.Application.Features.Auth.Mappers;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Application.Features.Chats.Mappers;
using Messenger.Application.Features.Users.DTO;
using Messenger.Application.Features.Users.Mappers;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Application.Extensions.DI
{
    public static class MapperExtensions
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            #region Users
            services.AddMapper<User, UserResponse, UserToUserResponseMapper>();
            services.AddMapper<User, ShortUserResponse, UserToShortUserResponseMapper>();
            services.AddMapper<RegisterCommand, Result<User>, RegisterCommandMapper>();
            services.AddMapper<User, ApplicationUser, UserToApplicationUserMapper>();
            #endregion

            #region Chats
            services.AddMapper<Chat, ChatResponse, ChatToChatResponseMapper>();
            services.AddMapper<Chat, ShortChatResponse, ChatToShortChatResponseMapper>();
            services.AddMapper<Message, MessageResponse, MessageToMessageResponseMapper>();
            services.AddMapper<SendMessageRequestModel, Result<Message>, SendMessageCommandMapper>();
            services.AddMapper<CreateChatRequestModel, Result<Chat>, CreateChatCommandMapper>();
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
