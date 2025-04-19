using api;
using api.Configuration;
using api.Helper;
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
    private readonly ILogger<Rsvp> _logger = Mock.Of<ILogger<Rsvp>>();
    private readonly Rsvp _function;
    private readonly IOptions<RsvpOptions> _options = Options.Create(new RsvpOptions("123456"));

    public RsvpTest()
    {
        _function = new Rsvp(_logger, _options);
    }

    [Fact]
    public void ItAcceptsPostRequests()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = request.Form = CreateFormCollection(_options.Value.InviteCode, "John Doe", "john@doe.de", 2);

        // Act
        var result = _function.Run(request);

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
    public void ItRejectsUnsupportedMethods(string method)
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = method;

        // Act
        var result = _function.Run(request);

        // Assert
        result.ShouldBeOfType<StatusCodeResult>()
            .StatusCode.ShouldBe(StatusCodes.Status405MethodNotAllowed);
    }

    [Fact]
    public void ItChecksForValidInviteCode()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = CreateFormCollection(_options.Value.InviteCode, "John Doe", "john@doe.de", 2);

        // Act
        var result = _function.Run(request);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public void ItRejectsOnInvalidInviteCode()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Method = "POST";
        request.Form = request.Form = CreateFormCollection("654321", "John Doe", "john@doe.de", 2);

        // Act
        var result = _function.Run(request);

        // Assert
        result.ShouldBeOfType<ForbidResult>();
    }

    private static FormCollection CreateFormCollection(string inviteCode, string name, string email, int extras)
        => new(new Dictionary<string, StringValues>
        {
            { FormKeys.InviteCode, inviteCode },
            { FormKeys.Name, name },
            { FormKeys.Email, email },
            { FormKeys.Extras, extras.ToString() }
        });
}
