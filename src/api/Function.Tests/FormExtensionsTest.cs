using api.Extensions;
using api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Shouldly;

namespace Function.Tests;

public class FormExtensionsTest
{
    private const string InviteCode = "123456";
    private const string Name = "John Doe";
    private const string Email = "john@doe.de";
    private const int Extras = 2;

    [Fact]
    public void ItThrowsIfInviteCodeIsMissing()
    {
        // Arrange
        var form = new FormCollection([]);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => form.GetRsvpRequest());
    }

    [Fact]
    public void ItThrowsIfNameIsMissing()
    {
        // Arrange
        var form = new FormCollection(new Dictionary<string, StringValues>
        {
            { FormKeys.InviteCode, InviteCode },
        });

        var action = () => form.GetRsvpRequest();

        // Act & Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void ItThrowsIfEmailIsMissing()
    {
        // Arrange
        var form = new FormCollection(new Dictionary<string, StringValues>
        {
            { FormKeys.InviteCode, InviteCode },
            { FormKeys.Name, Name },
        });

        var action = () => form.GetRsvpRequest();

        // Act & Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void ItThrowsIfExtrasIsMissing()
    {
        // Arrange
        var form = new FormCollection(new Dictionary<string, StringValues>
        {
            { FormKeys.InviteCode, InviteCode },
            { FormKeys.Name, Name },
            { FormKeys.Email, Email },
        });

        var action = () => form.GetRsvpRequest();

        // Act & Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void ItReturnsRsvpRequest()
    {
        // Arrange
        var form = new FormCollection(new Dictionary<string, StringValues>
        {
            { FormKeys.InviteCode, InviteCode },
            { FormKeys.Name, Name },
            { FormKeys.Email, Email },
            { FormKeys.Extras, Extras.ToString() },
        });
        // Act
        var result = form.GetRsvpRequest();

        // Assert
        result.ShouldNotBeNull();
        // result.InviteCode.ShouldBe("123456");
        result.Name.ShouldBe("John Doe");
        result.Email.ShouldBe("john@doe.de");
        result.Extras.ShouldBe(2);
    }
}