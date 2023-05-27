using System.Text;
using System.Text.Json;
using Identity.API.Models;
using Identity.Application.Models.Requests;
using Identity.Application.Models.User;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IJwtTokenHandlerService _jwtTokenHandlerService;
    private readonly IAccountService _accountService;
    private readonly HttpClient _httpClient;

    private const string LibraryUserApiEndpoint = "http://localhost:5164/api/library/user/register";
    
    public AccountController(IJwtTokenHandlerService jwtTokenHandlerService, IAccountService accountService, HttpClient httpClient)
    {
        _jwtTokenHandlerService = jwtTokenHandlerService;
        _accountService = accountService;
        _httpClient = httpClient;
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
        try
        {
            var account = await _jwtTokenHandlerService.RegisterAccount(registerRequest);

            try
            {
                var registerUserData = new
                {
                    id = account.Id,
                    username = account.Username
                };

                var registerUserJson = JsonSerializer.Serialize(registerUserData);
                var content = new StringContent(registerUserJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(LibraryUserApiEndpoint, content);
                response.EnsureSuccessStatusCode();

                return Ok(ApiResponse<bool>.Success(true));
            }
            catch (Exception e)
            {
                await RollbackOnAccountCreation(account.Id);

                return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new (null, e.Message) }));
            }
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new (null, e.Message) }));
        }
    }
    
    private async Task RollbackOnAccountCreation(int accountId)
    {
        await _accountService.DeleteAccountAsync(accountId);
    }
}