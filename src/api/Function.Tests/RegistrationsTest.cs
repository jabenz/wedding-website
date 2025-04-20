using api;
using api.Configuration;
using api.Entities;
using api.Helper;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Shouldly;

namespace Function.Tests;

public class RegistrationsTest
{
    private const string QueryCode = "123456";

    private readonly ILogger<Registrations> _logger = Mock.Of<ILogger<Registrations>>();
    private readonly ITableRepository _tableRepository = Mock.Of<ITableRepository>();
    private readonly IOptions<RegistrationsOptions> _options = Options.Create(new RegistrationsOptions()
    {
        QueryCode = QueryCode
    });
    private readonly Registrations _function;

    public RegistrationsTest()
    {
        _function = new Registrations(_logger, _options, _tableRepository);
    }

    [Fact]
    public async Task ItAcceptsGetRequests()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "GET";
        request.Query = new QueryCollection(new Dictionary<string, StringValues>
        {
            { RegistrationsKeys.QueryCode, QueryCode }
        });

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    [InlineData("PATCH")]
    [InlineData("OPTIONS")]
    [InlineData("HEAD")]
    [InlineData("TRACE")]
    [InlineData("CONNECT")]
    public async Task ItRejectsUnsupportedMethods(string method)
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = method;

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<StatusCodeResult>()
            .StatusCode.ShouldBe(StatusCodes.Status405MethodNotAllowed);
    }

    [Fact]
    public async Task ItRejectsOnWrongQueryCode()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "GET";
        request.Query = new QueryCollection(new Dictionary<string, StringValues>
        {
            { RegistrationsKeys.QueryCode, "wrong-code" }
        });

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<StatusCodeResult>()
            .StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public async Task ItReturnsExpectedResults()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "GET";
        request.Query = new QueryCollection(new Dictionary<string, StringValues>
        {
            { RegistrationsKeys.QueryCode, QueryCode }
        });

        Mock.Get(_tableRepository)
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(
            [
                new()
                {
                    RowKey = Guid.NewGuid().ToString(),
                    Name = "John Doe",
                    Email = "john@doe.de",
                    Extras = 2,
                    Timestamp = DateTimeOffset.UtcNow.AddDays(-1)
                },
                new()
                {
                    RowKey = Guid.NewGuid().ToString(),
                    Name = "Jane Doe",
                    Email = "jane@doe.de",
                    Extras = 0,
                    Timestamp = DateTimeOffset.UtcNow.AddDays(-5)
                },
            ]);

        // Act
        var result = await _function.RunAsync(request);
        var okResult = result.ShouldBeOfType<OkObjectResult>();
        var registrations = okResult.Value.ShouldBeAssignableTo<IEnumerable<RegistrationResponse>>();
        registrations.ShouldNotBeNull();
        registrations.ShouldNotBeEmpty();
        registrations.Count().ShouldBe(2);
        registrations.ShouldAllBe(r => r.Name == "John Doe" || r.Name == "Jane Doe");
    }
}

