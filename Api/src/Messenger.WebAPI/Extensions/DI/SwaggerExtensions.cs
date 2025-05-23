﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Messenger.WebAPI.Extensions.DI
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection ConfigureSwaggerGen(
            this IServiceCollection services)
        {
            services.AddSwaggerGen(
                ConfigureSwaggerAuthorization);

            return services;
        }

        private static void ConfigureSwaggerAuthorization(SwaggerGenOptions options)
        {
            options.ConfigureAppSecurityDefinitions();
            options.ConfigureAppSecurityRequirements();
        }

        private static SwaggerGenOptions ConfigureAppSecurityDefinitions(
            this SwaggerGenOptions options)
        {
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] { }
                    }
                });

            return options;
        }

        private static SwaggerGenOptions ConfigureAppSecurityRequirements(
            this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer { token }\""
                });

            return options;
        }
    }
}
