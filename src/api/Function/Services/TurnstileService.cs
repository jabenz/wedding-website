using System.Net;
using System.Text.Json;
using api.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Services;

public class TurnstileService(HttpClient client, IOptions<TurnstileOptions> options, ILogger<TurnstileService> logger) : ITurnstileService
{
    public async Task<bool> ValidateAsync(string token, CancellationToken ct = default)
    {
        try
        {
            var idempotencyKey = Guid.NewGuid().ToString();
            var formData = new Dictionary<string, string>
                {
                    { "secret", options.Value.SecretKey },
                    { "response", token },
                    { "idempotency_key", idempotencyKey }
                };

            var content = new FormUrlEncodedContent(formData);

            var response = await client.PostAsync("/turnstile/v0/siteverify", content, ct);
            var result = await response.Content.ReadAsStringAsync(ct);
            var resultObject = JsonSerializer.Deserialize<JsonElement>(result);
            var validationResult = resultObject.GetProperty("success").GetBoolean();

            logger.LogInformation("Turnstile validation result: {Result}, ik: {IdempotencyKey}", validationResult, idempotencyKey);

            return validationResult;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating Turnstile token");
            return false;
        }
    }
}