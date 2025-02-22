using Microsoft.AspNetCore.Mvc;

namespace SmartCache.Controllers;

[ApiController]
[Route("emails/{email}")]
public class EmailsController() : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmail(string email)
    {
      return Ok(email); 
    }

    [HttpPost]
    public IActionResult PostEmail(string email)
    {
        return Created(email, null);
    }
}