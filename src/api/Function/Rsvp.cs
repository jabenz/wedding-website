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
using api.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;
using api.Services;

namespace api;

public class Rsvp(ILogger<Rsvp> logger, /* IOptions<RsvpOptions> options, */ ITableRepository tableRepository, ITurnstileService turnstileService)
{
    private readonly ILogger<Rsvp> _logger = logger;

    [Function("Rsvp")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        _logger.LogInformation("RSVP function triggered (URI: {Uri})", req.GetDisplayUrl());

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

        var turnstileResponse = req.Form["cf-turnstile-response"].ToString();
        if (string.IsNullOrEmpty(turnstileResponse))
        {
            _logger.LogError("Turnstile response is missing in the request");
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        var isCaptchaValid = await turnstileService.ValidateAsync(turnstileResponse, CancellationToken.None);
        if (!isCaptchaValid)
        {
            _logger.LogError("Captcha validation failed");
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        // var host = req.Headers.Host.ToString();
        // if (host == null || !options.Value.AllowedHosts.Any(r => r == host))
        // {
        //     _logger.LogError("Invalid host: {Host}", host);
        //     return new StatusCodeResult(StatusCodes.Status403Forbidden);
        // }

        // if (options.Value.InviteCode == null)
        // {
        //     _logger.LogError("Invite code is not set in configuration");
        //     return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        // }


        RsvpRequest rsvpRequest;
        try
        {
            rsvpRequest = req.Form.GetRsvpRequest();
            _logger.LogInformation("Received RSVP request, Name: {Name}, Email: {Email}, Extras: {Extras}",
                rsvpRequest.Name, rsvpRequest.Email, rsvpRequest.Extras);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing RSVP request");
            return new BadRequestObjectResult("Invalid request format");
        }

        // if (rsvpRequest.InviteCode != options.Value.InviteCode)
        // {
        //     return new StatusCodeResult(StatusCodes.Status403Forbidden);
        // }

        _logger.LogInformation("Valid rsvp request");

        var rsvpEntity = new RegistrationEntity
        {
            Name = rsvpRequest.Name,
            Email = rsvpRequest.Email,
            Extras = rsvpRequest.Extras,
        };

        try
        {
            _logger.LogInformation("Saving RSVP to database: {RsvpEntity}", rsvpEntity);
            await tableRepository.CreateAsync(rsvpEntity);
        }
        catch (RegistrationAlreadyExistsException ex)
        {
            _logger.LogWarning(ex, "Registration already exists for email {Email}", rsvpEntity.Email);
            return new ConflictObjectResult("Registration already exists for this email");
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
