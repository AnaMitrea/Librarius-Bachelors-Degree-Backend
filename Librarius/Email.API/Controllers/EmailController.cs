using Email.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Email.API.Controllers;

[Route("[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailSender _emailSender;

    public EmailController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }
    
    // Route: /api/email/{id}/subscription
    [HttpPost("{authorId:int}/subscription")]
    public async Task<IActionResult> SendEmail(int authorId)
    {
        var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
            .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
        
        await _emailSender.SendAuthorSubscriptionEmailAsync(authorId, authorizationHeaderValue);
        return NoContent();
    }
}