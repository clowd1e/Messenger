using Messenger.AzureFunctions.Extensions;
using Messenger.Infrastructure.Extensions.DI;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddFunctionsOptions();
builder.Services.AddPersistence();
builder.Services.AddRepositories();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var host = builder.Build();

host.Run();
