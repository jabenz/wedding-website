using api;
using api.Entities;
using api.Exceptions;
using api.Helper;
using api.Repositories;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Shouldly;

namespace Function.Tests;

public class RsvpTest
{
    private const string Name = "John Doe";
    private const string Email = "john@doe.de";
    private const int Extras = 2;

    private readonly ILogger<Rsvp> _logger = Mock.Of<ILogger<Rsvp>>();
    private readonly ITableRepository _tableRepository = Mock.Of<ITableRepository>();
    private readonly ITurnstileService _turnstileService = Mock.Of<ITurnstileService>();
    private readonly Rsvp _function;

    public RsvpTest()
    {
        _function = new Rsvp(_logger, _tableRepository, _turnstileService);
        Mock.Get(_turnstileService)
            .Setup(service => service.ValidateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    [Fact]
    public async Task ItAcceptsPostRequests()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = request.Form = CreateFormCollection(Name, Email, Extras);

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
    }

    [Theory]
    [InlineData("GET")]
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
    public async Task ItRejectsOnWrongContentType()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.ContentType = "application/json";

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<StatusCodeResult>()
            .StatusCode.ShouldBe(StatusCodes.Status415UnsupportedMediaType);
    }

    [Fact]
    public async Task ItHandlesDatabaseErrorsGracefuly()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = request.Form = CreateFormCollection(Name, Email, Extras);

        Mock.Get(_tableRepository)
            .Setup(repo => repo.CreateAsync(It.IsAny<RegistrationEntity>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<StatusCodeResult>()
            .StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task ItSavesRsvpToDatabase()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = request.Form = CreateFormCollection(Name, Email, Extras);

        RegistrationEntity? rsvpEntityContainer = null!;
        Mock.Get(_tableRepository)
            .Setup(repo => repo.CreateAsync(It.IsAny<RegistrationEntity>()))
            .Callback<RegistrationEntity>(entity => rsvpEntityContainer = entity);

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
        Mock.Get(_tableRepository)
            .Verify(repo => repo.CreateAsync(It.IsAny<RegistrationEntity>()), Times.Once);
        rsvpEntityContainer.ShouldNotBeNull();
        rsvpEntityContainer.Name.ShouldBe(Name);
        rsvpEntityContainer.Email.ShouldBe(Email);
        rsvpEntityContainer.Extras.ShouldBe(Extras);
    }

    [Fact]
    public async Task ItHandlesExistingRsvpGracefully()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = request.Form = CreateFormCollection(Name, Email, Extras);

        Mock.Get(_tableRepository)
            .Setup(repo => repo.CreateAsync(It.IsAny<RegistrationEntity>()))
            .ThrowsAsync(new RegistrationAlreadyExistsException(Email));

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<ConflictObjectResult>();
    }

    private static FormCollection CreateFormCollection(string name, string email, int extras)
        => new(new Dictionary<string, StringValues>
        {
            { FormKeys.Name, name },
            { FormKeys.Email, email },
            { FormKeys.Extras, extras.ToString() },
            { "cf-turnstile-response", "some-token" }
        });
}
