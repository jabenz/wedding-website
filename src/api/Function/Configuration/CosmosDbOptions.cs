namespace api.Configuration;

public record CosmosDbOptions
{
    public required string ConnectionString { get; init; }
    public required string TableName { get; init; }
}