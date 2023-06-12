using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Identity.API.Models;
using Identity.API.Utils;
using Identity.Application.Models.Requests;
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
    private readonly HttpClient _httpClient;

    public UserController(IAccountService accountService, IUserService userService, HttpClient httpClient)
    {
        _accountService = accountService;
        _userService = userService;
        _httpClient = httpClient;
    }
    
    // Route: /api/user/information
    [HttpGet("information")]
    public async Task<IActionResult> GetUserInformation()
    {
        try
        {
            var username = GetUsernameFromToken();
            
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
            var username = GetUsernameFromToken();
            
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
            var username = GetUsernameFromToken();
            
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
            var username = GetUsernameFromToken();
            
            var response = await _userService.FindUserIdByUsernameAsync(username);

            return Ok(ApiResponse<int>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<int>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
    
    // Route: /api/user/points/add
    [HttpPut("points/add")]
    public async Task<IActionResult> AddPointsToUser([FromBody] OnlyPointsModel onlyPointsModel)
    {
        try
        {
            var username = GetUsernameFromToken();
            
            var userTotalPoints = await _userService.AddPointsToUserAsync(username, onlyPointsModel.Points);
            
            var requestModel = new 
            {
                Points = userTotalPoints
            };

            var requestJson = JsonSerializer.Serialize(requestModel);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAuthorizationHeaderValue());
            var httpResponse = await _httpClient.PostAsync(
                "http://localhost:5164/api/level/points",
                requestContent
            );
            httpResponse.EnsureSuccessStatusCode();
            
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var newLevel = Utilities.GetJsonPropertyAsString(jsonResponse, new[] { "result" });
            
            var lvl = await _userService.SetUserLevelAsync(username, newLevel);

            return Ok(ApiResponse<string>.Success(lvl));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<int>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }

    public string GetUsernameFromToken()
    {
        var authorizationHeaderValue = GetAuthorizationHeaderValue();
        return Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
    }
    
    public string GetAuthorizationHeaderValue()
    {
        return HttpContext.Request.Headers[HeaderNames.Authorization]
            .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
    }
}