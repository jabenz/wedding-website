namespace api.Configuration;

public record RsvpOptions()
{
    public required string InviteCode { get; init; }
};