namespace api.Configuration;

public record TurnstileOptions
{
    public required string SecretKey { get; init; }
}