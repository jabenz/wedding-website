using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Data.Tables;

namespace api.Entities;

public class RegistrationEntity : ITableEntity
{
    // Technical values
    public string PartitionKey { get; set; } = "Rsvp";
    // We use the email as the RowKey, so we can use it to query the entity and provide uniqueness of entities based on email
    public string RowKey { get => Email; set => Email = value; }
    public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;
    public ETag ETag { get; set; }

    // Domain values
    public required string Name { get; set; }
    [EmailAddress] public required string Email { get; set; }
    public int Extras { get; set; }
}