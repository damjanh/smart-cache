using Microsoft.AspNetCore.Mvc;
using SmartCache.Services;

namespace SmartCache.Controllers;

[ApiController]
[Route("emails/{email}")]
public class EmailsController(IEmailsService service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmail(string email)
    {
        var foundEmail =  service.GetEmail(email);
        return string.IsNullOrEmpty(foundEmail) ? NotFound() : Ok(foundEmail); 
    }

    [HttpPost]
    public IActionResult PostEmail(string email)
    {
        var success = service.SetEmail(email);
        return success ? Created() : Conflict();
    }
}