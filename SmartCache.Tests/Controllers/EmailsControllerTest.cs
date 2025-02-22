using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCache.Controllers;
using SmartCache.Services;
using Xunit;

namespace SmartCache.Tests.Controllers;

public class EmailsControllerTests
{
    private readonly Mock<IEmailsService> _mockEmailsService;
    private readonly EmailsController _controller;

    public EmailsControllerTests()
    {
        _mockEmailsService = new Mock<IEmailsService>();
        _controller = new EmailsController(_mockEmailsService.Object);
    }

    [Fact]
    public async Task GetEmail_ReturnsOk_WhenEmailExists()
    {
        const string email = "test@example.com";
        _mockEmailsService.Setup(s => s.GetEmail(email)).ReturnsAsync(email);

        var result = await _controller.GetEmail(email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(email, okResult.Value);
    }

    [Fact]
    public async Task GetEmail_ReturnsNotFound_WhenEmailDoesNotExist()
    {
        const string email = "notfound@example.com";
        _mockEmailsService.Setup(s => s.GetEmail(email)).ReturnsAsync("");

        var result = await _controller.GetEmail(email);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PostEmail_ReturnsCreated_WhenEmailIsSetSuccessfully()
    {
        const string email = "new@example.com";
        _mockEmailsService.Setup(s => s.SetEmail(email)).ReturnsAsync(true);

        var result = await _controller.PostEmail(email);

        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task PostEmail_ReturnsConflict_WhenEmailSetFails()
    {
        const string email = "conflict@example.com";
        _mockEmailsService.Setup(s => s.SetEmail(email)).ReturnsAsync(false);

        var result = await _controller.PostEmail(email);

        Assert.IsType<ConflictResult>(result);
    }
}