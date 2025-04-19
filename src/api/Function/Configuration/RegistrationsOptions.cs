namespace api.Configuration;

public record RegistrationsOptions()
{
    public required string QueryCode { get; init; }
};