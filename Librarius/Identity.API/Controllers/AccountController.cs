using System.IdentityModel.Tokens.Jwt;
using Identity.API.Models;
using Identity.Application.Models.Requests;
using Identity.Application.Models.User;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Identity.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IJwtTokenHandlerService _jwtTokenHandlerService;
    private readonly IAccountService _accountService;
    private readonly IAccountActivityService _accountActivityService;

    public AccountController(
        IJwtTokenHandlerService jwtTokenHandlerService,
        IAccountService accountService,
        IAccountActivityService accountActivityService)
    {
        _jwtTokenHandlerService = jwtTokenHandlerService;
        _accountService = accountService;
        _accountActivityService = accountActivityService;
    }
    
    private static string ExtractUsernameFromAccessToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var username = jwtSecurityToken.Claims.First(claim => claim.Type == "name").Value;

        return username;
    }
    
    // Route: /api/account/
    [HttpGet("")]
    public async Task<IActionResult> GetUserInformation()
    {
        var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var username = ExtractUsernameFromAccessToken(authorizationHeaderValue);
        
        try
        {
            var response = await _accountService.GetUserInformationAsync(username);

            if (response == null) throw new Exception("Access Token Invalid.");

            return Ok(ApiResponse<UserModel>.Success(response));
        }
        catch (Exception e)
        {
            return NotFound(ApiResponse<UserModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }

    // Route: /api/account/login
    [HttpPut("login")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequestModel authenticationRequest)
    {
        try
        {
            var response = await _jwtTokenHandlerService.AuthenticateAccount(authenticationRequest);
            if (response == null) return Unauthorized();
            
            var account = await _accountActivityService.UpdateUserActivity(response.Username);
            if (account == null) throw new Exception("Invalid User Information.");
            
            return Ok(ApiResponse<AuthJwtResponseModel>.Success(new AuthJwtResponseModel
            {
                JwtToken = response.JwtToken
            }));
        }
        catch (Exception e)
        {
            return NotFound(ApiResponse<AuthenticationResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
}