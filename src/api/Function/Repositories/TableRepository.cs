using System.Diagnostics.CodeAnalysis;
using api.Configuration;
using api.Entities;
using Azure.Data.Tables;
using Microsoft.Extensions.Options;

namespace api.Repositories;

public interface ITableRepository
{
    Task<IEnumerable<RsvpEntity>> GetAllAsync();
    Task CreateAsync(RsvpEntity entity);
}

[ExcludeFromCodeCoverage(Justification = "We don't need to test the repository")]
public class TableRepository : ITableRepository
{
    private readonly TableClient _tableClient;

    public TableRepository(IOptions<CosmosDbOptions> options)
    {
        var serviceClient = new TableServiceClient(options.Value.ConnectionString);
        _tableClient = serviceClient.GetTableClient(options.Value.TableName);
    }

    public async Task CreateAsync(RsvpEntity entity)
    {
        await _tableClient.AddEntityAsync(entity);
    }

    public async Task<IEnumerable<RsvpEntity>> GetAllAsync()
    {
        List<RsvpEntity> allEntities = [];

        await foreach (RsvpEntity entity in _tableClient.QueryAsync<RsvpEntity>())
        {
            allEntities.Add(entity);
        }

        return allEntities;
    }
}