using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using api.Configuration;
using api.Extensions;
using api.Models;
using api.Repositories;
using api.Entities;

namespace api;

public class Rsvp(ILogger<Rsvp> logger, IOptions<RsvpOptions> options, ITableRepository tableRepository)
{
    private readonly ILogger<Rsvp> _logger = logger;

    [Function("Rsvp")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        if (options.Value.InviteCode == null)
        {
            _logger.LogError("Invite code is not set in configuration");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        _logger.LogInformation("Processing RSVP request");
        if (!HttpMethods.IsPost(req.Method))
        {
            _logger.LogWarning("Request method {Method} not allowed", req.Method);
            return new StatusCodeResult(StatusCodes.Status405MethodNotAllowed);
        }

        if (!req.HasFormContentType)
        {
            _logger.LogWarning("Request content type {ContentType} not allowed", req.ContentType);
            return new StatusCodeResult(StatusCodes.Status415UnsupportedMediaType);
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
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        _logger.LogInformation("Valid rsvp request");

        var rsvpEntity = new RsvpEntity
        {
            Name = rsvpRequest.Name,
            Email = rsvpRequest.Email,
            Extras = rsvpRequest.Extras
        };

        try
        {
            _logger.LogInformation("Saving RSVP to database: {RsvpEntity}", rsvpEntity);
            await tableRepository.CreateAsync(rsvpEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving RSVP to database");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        _logger.LogInformation("RSVP saved successfully");
        return new OkObjectResult("Invite accepted");
    }
}
