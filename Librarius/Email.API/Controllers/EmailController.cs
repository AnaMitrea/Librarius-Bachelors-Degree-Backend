using Email.Application.Models;
using Email.Application.Services;
using Microsoft.AspNetCore.Mvc;

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
        await _emailSender.SendEmailAsync(authorId);
        return NoContent();
    }
}