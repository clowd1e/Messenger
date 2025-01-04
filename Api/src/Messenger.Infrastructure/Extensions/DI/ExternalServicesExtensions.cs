using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Messenger.Application.Abstractions.Storage;
using Messenger.Infrastructure.Extensions.DI.Shared;
using Messenger.Infrastructure.External.Blobs;
using Messenger.Infrastructure.External.Blobs.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Extensions.DI
{
    public static class ExternalServicesExtensions
    {
        public static IServiceCollection AddExternalServices(
            this IServiceCollection services)
        {
            services.AddAzureServiceClient();

            services.AddSingleton<IBlobService, AzureBlobService>();

            return services;
        }

        private static IServiceCollection AddAzureServiceClient(
            this IServiceCollection services)
        {
            services.AddSingleton((serviceProvider) =>
            {
                var blobStorageSettings = serviceProvider.GetOptions<AzureBlobStorageSettings>();

                var blobServiceClient = new BlobServiceClient(blobStorageSettings.ConnectionString);

                var containerClient = blobServiceClient.GetBlobContainerClient(blobStorageSettings.ContainerName);

                containerClient.CreateIfNotExists();

                var properties = containerClient.GetProperties();

                if (ContainerHasPublicAccess(properties.Value))
                {
                    containerClient.SetAccessPolicy(PublicAccessType.Blob);
                }

                return blobServiceClient;
            });

            return services;
        }

        private static bool ContainerHasPublicAccess(
            BlobContainerProperties properties)
        {
            return properties.PublicAccess != PublicAccessType.Blob;
        }
    }
}
