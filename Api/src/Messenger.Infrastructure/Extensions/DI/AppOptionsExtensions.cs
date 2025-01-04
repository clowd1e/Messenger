﻿using Messenger.Application.Images.Options;
using Messenger.Infrastructure.Authentication.Options;
using Messenger.Infrastructure.Extensions.DI.Shared;
using Messenger.Infrastructure.External.Blobs.Options;
using Messenger.Infrastructure.Persistense.Options;
using Messenger.Infrastructure.Services.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Extensions.DI
{
    public static class AppOptionsExtensions
    {
        public static IServiceCollection AddAppOptions(
            this IServiceCollection services)
        {
            services.ConfigureValidatableOnStartOptions<DbSettings>(
                DbSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<HashSettings>(
                HashSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<JwtSettings>(
                JwtSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<LoginSettings>(
                LoginSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<AzureBlobStorageSettings>(
                AzureBlobStorageSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<ImageSettings>(
                ImageSettings.SectionName);

            return services;
        }
    }
}
