using System.Net;

namespace api.Services;

public interface ITurnstileService
{
    Task<bool> ValidateAsync(string token, string idempotencyKey, CancellationToken ct = default);
}