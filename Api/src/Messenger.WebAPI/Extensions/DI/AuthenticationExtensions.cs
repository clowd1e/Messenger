﻿using Messenger.Infrastructure.Authentication.Options;
using Messenger.Infrastructure.Extensions.DI.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Messenger.WebAPI.Extensions.DI
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigureAuthentication(
            this IServiceCollection services)
        {
            var jwtSettings = services.BuildServiceProvider().GetOptions<JwtSettings>();
            var signingKey = CreateSigningKey(jwtSettings);

            services.AddAuthenticationWithJwtBearer(jwtSettings, signingKey);

            return services;
        }

        private static SymmetricSecurityKey CreateSigningKey(JwtSettings jwtSettings)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey));
        }

        private static IServiceCollection AddAuthenticationWithJwtBearer(
           this IServiceCollection services,
           JwtSettings jwtSettings,
           SymmetricSecurityKey signingKey)
        {
            services.AddAuthentication(
                    ConfigureAuthenticationOptions())
                .AddJwtBearer(
                    ConfigureJwtBearerOptions(jwtSettings, signingKey));

            return services;
        }

        private static Action<AuthenticationOptions> ConfigureAuthenticationOptions()
        {
            return options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            };
        }

        private static Action<JwtBearerOptions> ConfigureJwtBearerOptions(
            JwtSettings jwtSettings,
            SymmetricSecurityKey signingKey)
        {
            return options =>
            {
                options.TokenValidationParameters = CreateTokenValidationParameters(
                    jwtSettings,
                    signingKey);
                options.Events = ConfigureJwtBearerEvents();
            };
        }

        private static JwtBearerEvents ConfigureJwtBearerEvents()
        {
            return new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        }

        private static TokenValidationParameters CreateTokenValidationParameters(
            JwtSettings jwtSettings,
            SymmetricSecurityKey signingKey)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = signingKey
            };
        }
    }
}
