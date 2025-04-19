using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using api.Configuration;
using api.Extensions;
using api.Models;

namespace api;

public class Rsvp(ILogger<Rsvp> logger, IOptions<RsvpOptions> options)
{
    private readonly ILogger<Rsvp> _logger = logger;

    [Function("Rsvp")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        _logger.LogInformation("Processing RSVP request");
        if (!HttpMethods.IsPost(req.Method))
        {
            _logger.LogWarning("Request method {Method} not allowed", req.Method);
            return new StatusCodeResult(StatusCodes.Status405MethodNotAllowed);
        }

        RsvpRequest rsvpRequest;
        try
        {
            rsvpRequest = req.Form.GetRsvpRequest();
            _logger.LogInformation("Received RSVP request: {RsvpRequest}", rsvpRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing RSVP request");
            return new BadRequestObjectResult("Invalid request format");
        }

        if (rsvpRequest.InviteCode != options.Value.InviteCode)
        {
            return new ForbidResult();
        }

        _logger.LogInformation("Valid rsvp request");

        return new OkObjectResult("Invite accepted");
    }
}
