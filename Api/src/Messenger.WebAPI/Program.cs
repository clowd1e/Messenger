using Messenger.Application;
using Messenger.Infrastructure;
using Messenger.Infrastructure.Extensions;
using Messenger.WebAPI.Extensions.DI;
using Messenger.WebAPI.Factories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.ConfigureCors(builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>()!);

builder.Services.AddControllers().ConfigureJsonOptions();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.ConfigureAuthentication();

builder.Services.ConfigureSignalR();

builder.Services.AddSingleton<ProblemDetailsFactory>();

builder.Services.AddLogging(logs =>
{
    logs.AddConsole();
    logs.AddAzureWebAppDiagnostics();
});

var app = builder.Build();

app.AddMiddleware();

app.ApplyMigration();

app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapSignalRHubs();

app.MapControllers();

app.Run();
