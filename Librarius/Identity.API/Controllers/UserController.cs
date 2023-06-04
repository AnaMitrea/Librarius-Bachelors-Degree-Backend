using Identity.API.Models;
using Identity.API.Utils;
using Identity.Application.Models.User;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Identity.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IUserService _userService;

    public UserController(IAccountService accountService, IUserService userService)
    {
        _accountService = accountService;
        _userService = userService;
    }
    
    // Route: /api/user/information
    [HttpGet("information")]
    public async Task<IActionResult> GetUserInformation()
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _accountService.GetUserInformationAsync(username);

            if (response == null) throw new Exception("Access Token Invalid.");

            return Ok(ApiResponse<DashboardUserModel>.Success(response));
        }
        catch (Exception e)
        {
            return NotFound(ApiResponse<DashboardUserModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/user/email
    [HttpGet("email")]
    public async Task<IActionResult> GetUserEmail()
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _accountService.GeUserEmailAsync(username);

            if (response == null) throw new Exception("Invalid information.");

            return Ok(ApiResponse<string>.Success(response));
        }
        catch (Exception e)
        {
            return NotFound(ApiResponse<string>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/user/all/points
    [HttpGet("all/points")]
    public async Task<IActionResult> GetAllUsersByPointsDesc()
    {
        try
        {
            var response = await _userService.GetAllUsersByPointsDescAsync();

            return Ok(ApiResponse<IEnumerable<UserLeaderboardByPoints>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<UserLeaderboardByPoints>>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
    
    
    // Route: /api/user/dashboard/activity
    [HttpGet("dashboard/activity")]
    public async Task<IActionResult> GetUserDashboardActivity()
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _userService.GetUserDashboardActivityAsync(username);

            return Ok(ApiResponse<IEnumerable<string>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<string>>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
    
    // Route: /api/user/id
    [HttpGet("id")]
    public async Task<IActionResult> FindUserIdByUsername()
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _userService.FindUserIdByUsernameAsync(username);

            return Ok(ApiResponse<int>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<int>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
}