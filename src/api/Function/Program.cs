using api.Configuration;
using api.Repositories;
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
    .AddOptions<RsvpOptions>()
    .BindConfiguration(nameof(RsvpOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

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

builder.Services.AddScoped<ITableRepository, TableRepository>();

builder.Build().Run();
