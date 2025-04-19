using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Data.Tables;

namespace api.Entities;

public class RsvpEntity : ITableEntity
{
    // Technical values
    public string RowKey { get; set; } = Guid.NewGuid().ToString();
    public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;
    public ETag ETag { get; set; }
    public string PartitionKey { get; set; } = "Rsvp";

    // Domain values
    public required string Name { get; set; }
    [EmailAddress] public required string Email { get; set; }
    public int Extras { get; set; }
}