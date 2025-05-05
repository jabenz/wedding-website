using api;
using api.Configuration;
using api.Entities;
using api.Exceptions;
using api.Helper;
using api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Shouldly;

namespace Function.Tests;

public class RsvpTest
{
    private const string Name = "John Doe";
    private const string Email = "john@doe.de";
    private const int Extras = 2;
    private const string InviteCode = "123456";

    private readonly ILogger<Rsvp> _logger = Mock.Of<ILogger<Rsvp>>();
    private readonly ITableRepository _tableRepository = Mock.Of<ITableRepository>();
    private readonly Rsvp _function;
    private readonly IOptions<RsvpOptions> _options = Options.Create(new RsvpOptions()
    {
        InviteCode = InviteCode,
        AllowedHosts = ["localhost", ""]
    });

    public RsvpTest()
    {
        _function = new Rsvp(_logger, _options, _tableRepository);
    }

    [Fact]
    public async Task ItAcceptsPostRequests()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = request.Form = CreateFormCollection(InviteCode, Name, Email, Extras);

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

    // [Fact]
    // public async Task ItChecksForValidInviteCode()
    // {
    //     // Arrange
    //     var request = new DefaultHttpContext().Request;
    //     request.Method = "POST";
    //     request.Form = CreateFormCollection(_options.Value.InviteCode, Name, Email, Extras);

    //     // Act
    //     var result = await _function.RunAsync(request);

    //     // Assert
    //     result.ShouldBeOfType<OkObjectResult>();
    // }

    // [Fact]
    // public async Task ItRejectsOnInvalidInviteCode()
    // {
    //     // Arrange
    //     var request = new DefaultHttpContext().Request;
    //     request.Method = "POST";
    //     request.Form = request.Form = CreateFormCollection("654321", Name, Email, Extras);

    //     // Act
    //     var result = await _function.RunAsync(request);

    //     // Assert
    //     result.ShouldBeOfType<StatusCodeResult>()
    //         .StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
    // }

    [Fact]
    public async Task ItHandlesDatabaseErrorsGracefuly()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = request.Form = CreateFormCollection(InviteCode, Name, Email, Extras);

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
        request.Form = request.Form = CreateFormCollection(InviteCode, Name, Email, Extras);

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
        request.Form = request.Form = CreateFormCollection(InviteCode, Name, Email, Extras);

        Mock.Get(_tableRepository)
            .Setup(repo => repo.CreateAsync(It.IsAny<RegistrationEntity>()))
            .ThrowsAsync(new RegistrationAlreadyExistsException(Email));

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<ConflictObjectResult>();
    }

    [Fact]
    public async Task ItRejectsOnUnexpectedHost()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = request.Form = CreateFormCollection(InviteCode, Name, Email, Extras);
        request.Host = new HostString("unexpected-host.com");

        // Act
        var result = await _function.RunAsync(request);

        // Assert
        result.ShouldBeOfType<StatusCodeResult>()
            .StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
    }


    private static FormCollection CreateFormCollection(string inviteCode, string name, string email, int extras)
        => new(new Dictionary<string, StringValues>
        {
            { FormKeys.InviteCode, inviteCode },
            { FormKeys.Name, name },
            { FormKeys.Email, email },
            { FormKeys.Extras, extras.ToString() },
        });
}
