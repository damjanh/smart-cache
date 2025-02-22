using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCache.Services;

namespace SmartCache.Controllers;

[Authorize]
[ApiController]
[Route("emails/{email}")]
public class EmailsController(IEmailsService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEmail(string email)
    {
        var foundEmail = await service.GetEmail(email);
        return string.IsNullOrEmpty(foundEmail) ? NotFound() : Ok(foundEmail); 
    }

    [HttpPost]
    public async Task<IActionResult> PostEmail(string email)
    {
        var success = await service.SetEmail(email);
        return success ? Created() : Conflict();
    }
}