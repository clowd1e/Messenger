using Messenger.Infrastructure;
using Messenger.Infrastructure.Extensions;
using Messenger.Application;
using Messenger.WebAPI.Extensions.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.ConfigureAuthentication();

var app = builder.Build();

app.AddMiddleware();

app.ApplyMigration();
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
