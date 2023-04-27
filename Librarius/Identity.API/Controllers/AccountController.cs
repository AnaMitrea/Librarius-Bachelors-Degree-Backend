using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Identity.API.Models;
using Identity.Application.Models;
using Identity.Application.Models.Requests;
using Identity.Application.Models.User;
using Identity.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Identity.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IJwtTokenHandlerService _jwtTokenHandlerService;
    private readonly IAccountService _accountService;

    public AccountController(IJwtTokenHandlerService jwtTokenHandlerService, IAccountService accountService)
    {
        _jwtTokenHandlerService = jwtTokenHandlerService;
        _accountService = accountService;
    }
    
    // Route: /api/account/
    [HttpGet("")]
    public async Task<IActionResult> GetUserInformation()
    {
        var accessToken = HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var username = jwtSecurityToken.Claims.First(claim => claim.Type == "name").Value;

        var response = await _accountService.GetUserInformationAsync(username);
        
        if (response == null) throw new Exception("Access Token Invalid.");
        
        return Ok(ApiResponse<UserModel>.Success(response));
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