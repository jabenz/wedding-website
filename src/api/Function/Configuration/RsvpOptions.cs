namespace api.Configuration;

public record RsvpOptions()
{
    public required string InviteCode { get; init; }
    public required string[] AllowedHosts { get; init; } = [];
};