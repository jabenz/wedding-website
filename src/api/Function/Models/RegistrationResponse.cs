namespace api.Models;

public record RegistrationResponse
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int Extras { get; init; }
    public DateTime CreatedAt { get; init; }
}