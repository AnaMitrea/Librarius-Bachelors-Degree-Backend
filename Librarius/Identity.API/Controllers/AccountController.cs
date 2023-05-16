using Identity.API.Models;
using Identity.Application.Models.Requests;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

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

    // Route: /api/account/login
    [HttpPut("login")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequestModel authenticationRequest)
    {
        try
        {
            var response = await _jwtTokenHandlerService.AuthenticateAccount(authenticationRequest);
            if (response == null) return Unauthorized();
            
            var account = await _accountService.UpdateUserActivity(response.Username);
            if (account == null) throw new Exception("Invalid User Information.");
            
            return Ok(ApiResponse<AuthJwtResponseModel>.Success(new AuthJwtResponseModel
            {
                JwtToken = response.JwtToken
            }));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<AuthenticationResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/account/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerRequest)
    {
        // todo Add Created User to Account & LibraryUser table!!!!
        
        try
        {
            var response = await _jwtTokenHandlerService.RegisterAccount(registerRequest);
          
            return Ok(ApiResponse<AuthJwtResponseModel>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<AuthenticationResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
}