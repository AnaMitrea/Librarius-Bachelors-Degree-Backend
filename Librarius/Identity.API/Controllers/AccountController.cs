using Identity.API.Models;
using Identity.Application.Models;
using Identity.Application.Models.Requests;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IJwtTokenHandlerService _jwtTokenHandlerService;

    public AccountController(IJwtTokenHandlerService jwtTokenHandlerService)
    {
        _jwtTokenHandlerService = jwtTokenHandlerService;
    }

    // Route: /api/account/login
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequestModel authenticationRequest)
    {
        var response = await _jwtTokenHandlerService.AuthenticateAccount(authenticationRequest);
        if (response == null) return Unauthorized();
        
        return Ok(ApiResponse<AuthenticationResponseModel>.Success(response));
    }
}