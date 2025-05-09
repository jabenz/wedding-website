using api.Configuration;
using api.Repositories;
using api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services
    .AddOptions<CosmosDbOptions>()
    .BindConfiguration(nameof(CosmosDbOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<RegistrationsOptions>()
    .BindConfiguration(nameof(RegistrationsOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<TurnstileOptions>()
    .BindConfiguration(nameof(TurnstileOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<ITurnstileService, TurnstileService>(client =>
{
    client.BaseAddress = new Uri("https://challenges.cloudflare.com");
});

builder.Services.AddScoped<ITableRepository, TableRepository>();

builder.Build().Run();
