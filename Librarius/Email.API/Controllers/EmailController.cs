using Microsoft.AspNetCore.Mvc;

namespace Email.API.Controllers;

[Route("[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    //private readonly IService _Service;

    public EmailController()
    {
        
    }
    
    // Route: /api/email
    [HttpGet("")]
    public async Task<IActionResult> GetAsync()
    {
        throw new NotImplementedException();
    }
}