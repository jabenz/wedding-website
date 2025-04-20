using System.Diagnostics.CodeAnalysis;
using api.Configuration;
using api.Entities;
using api.Exceptions;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Options;

namespace api.Repositories;

public interface ITableRepository
{
    Task<IEnumerable<RegistrationEntity>> GetAllAsync();
    Task CreateAsync(RegistrationEntity entity);
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

    public async Task CreateAsync(RegistrationEntity entity)
    {
        try
        {
            await _tableClient.AddEntityAsync(entity);
        }
        catch (RequestFailedException ex) when (ex.Status == 409)
        {
            throw new RegistrationAlreadyExistsException(entity.Email, ex);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException("Error creating entity", ex);
        }
    }

    public async Task<IEnumerable<RegistrationEntity>> GetAllAsync()
    {
        List<RegistrationEntity> allEntities = [];

        await foreach (RegistrationEntity entity in _tableClient.QueryAsync<RegistrationEntity>())
        {
            allEntities.Add(entity);
        }

        return allEntities;
    }
}