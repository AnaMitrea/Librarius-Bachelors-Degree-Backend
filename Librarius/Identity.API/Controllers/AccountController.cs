using System.Text;
using System.Text.Json;
using Identity.API.Models;
using Identity.API.Utils;
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
    private readonly ITriggerRewardService _triggerRewardService;
    private readonly HttpClient _httpClient;

    private const string LibraryUserApiEndpoint = "http://localhost:5164/api/library/user/register";
    private const string WelcomeEmailApiEndpoint = "http://localhost:5164/api/email/welcome";
    private const string JoinLoginTrophyApiEndpoint = "http://localhost:5164/api/trophy/join/20";
    
    public AccountController(
        IJwtTokenHandlerService jwtTokenHandlerService,
        IAccountService accountService,
        HttpClient httpClient,
        ITriggerRewardService triggerRewardService)
    {
        _jwtTokenHandlerService = jwtTokenHandlerService;
        _accountService = accountService;
        _httpClient = httpClient;
        _triggerRewardService = triggerRewardService;
    }

    // Route: /api/account/login
    [HttpPut("login")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequestModel authenticationRequest)
    {
        try
        {
            var response = await _jwtTokenHandlerService.AuthenticateAccount(authenticationRequest);
            if (response == null) return Unauthorized();
            
            var isFirstLoginEver = await _accountService.IsFirstLoginEver(authenticationRequest.Username);
            
            var account = await _accountService.UpdateUserActivity(response.Username);
            if (account == null) throw new Exception("Invalid User Information.");
            
            string criterion;
            if (isFirstLoginEver)
            {
                criterion = "login";
                await _triggerRewardService.TriggerJoinFirstLoginTrophy(response.JwtToken);
            }
            else
            {
                criterion = Utilities.CheckDate(DateTime.Now.Date);
            }
            
            var hasWonAny = false;
            if (!string.IsNullOrEmpty(criterion))
            {
                hasWonAny = await _triggerRewardService.TriggerUpdateActivity(criterion, true, response.JwtToken);
            }
            
            return Ok(ApiResponse<AuthJwtResponseModel>.Success(new AuthJwtResponseModel
            {
                JwtToken = response.JwtToken,
                HasWon = hasWonAny
            }));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<AuthJwtResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
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

                var welcomeEmailData = new
                {
                    registerRequest.Username,
                    registerRequest.Email
                };
                
                var email = await _httpClient.PostAsJsonAsync(WelcomeEmailApiEndpoint, welcomeEmailData);
                email.EnsureSuccessStatusCode();
                
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