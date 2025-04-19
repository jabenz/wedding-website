using api.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Function.Tests;

public class UnitTest1
{
    [Fact]
    public void ItAcceptsPostRequests()
    {
        var loggerMock = Mock.Of<ILogger<Rsvp>>();
        var function = new Rsvp(loggerMock);

        var request = new DefaultHttpContext().Request;
        request.Method = "POST";

        var result = function.Run(request);
        Assert.IsType<OkObjectResult>(result);
    }
}
