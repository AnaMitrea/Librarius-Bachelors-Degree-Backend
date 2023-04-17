using Identity.Application;
using Identity.Application.Models;
using Identity.Application.Models.Requests;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly JwtTokenHandler _jwtTokenHandler;

    public AccountController(JwtTokenHandler jwtTokenHandler)
    {
        _jwtTokenHandler = jwtTokenHandler;
    }

    // Route: /api/account/login
    [HttpPost("login")]
    public ActionResult<AuthenticationResponseModel?> Authenticate([FromBody] AuthenticationRequestModel authenticationRequest)
    {
        var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(authenticationRequest);
        if (authenticationResponse == null) return Unauthorized();
        
        return authenticationResponse;
    }
}