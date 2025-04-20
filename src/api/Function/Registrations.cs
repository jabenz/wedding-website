using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using api.Repositories;
using api.Configuration;
using api.Extensions;
using api.Helper;

namespace api;

public class Registrations(ILogger<Registrations> logger, IOptions<RegistrationsOptions> options, ITableRepository tableRepository)
{

    [Function("Registrations")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        if (options.Value.QueryCode == null)
        {
            logger.LogError("QueryCode code is not set in configuration");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        logger.LogInformation("Processing Registrations request");
        if (!HttpMethods.IsGet(req.Method))
        {
            logger.LogWarning("Request method {Method} not allowed", req.Method);
            return new StatusCodeResult(StatusCodes.Status405MethodNotAllowed);
        }

        if (!req.Query.TryGetValue(RegistrationsKeys.QueryCode, out var queryCode) || queryCode != options.Value.QueryCode)
        {
            logger.LogWarning("Invalid query code provided");
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        try
        {
            logger.LogInformation("Querying registrations from database");
            var registrations = await tableRepository.GetAllAsync();

            return new OkObjectResult(registrations.Select(r => r.ToResponse()).ToList());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error querying registrations from database");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
