using SmartCache.Validation;
using Xunit;

namespace SmartCache.Tests.Validation;

public class EmailValidatorTests
{
    [Theory]
    [InlineData("test@example.com", true)]      // Valid
    [InlineData("test@example", false)]         // Missing domain
    [InlineData("test@.com", false)]            // Missing username
    [InlineData("test@com", false)]             // Missing dot in domain part
    [InlineData("test@subdomain.example.com", true)]  // Valid, with subdomain
    [InlineData("email", false)]                // No @ symbol
    [InlineData("", false)]                     // Empty string
    [InlineData(null, false)]                   // Null
    [InlineData("   ", false)]                  // Only spaces
    public void IsValidEmail_ReturnsExpectedResult(string email, bool expected)
    {
        var result = EmailValidator.IsValidEmail(email);
        Assert.Equal(expected, result);
    }
}