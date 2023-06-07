using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Trophy.API.Models;
using Trophy.API.Utils;
using Trophy.Application.Models.LevelAssign.Request;
using Trophy.Application.Models.LevelAssign.Response;
using Trophy.Application.Services;

namespace Trophy.API.Controllers;

[Route("level")]
[ApiController]
public class LevelAssignController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILevelAssignService _levelAssignService;
    
    private const string UserIdApiUrl = "http://localhost:5164/api/user/id";

    public LevelAssignController(ILevelAssignService levelAssignService, HttpClient httpClient)
    {
        _levelAssignService = levelAssignService;
        _httpClient = httpClient;
    }
    
    // Route: /api/level/all?asc=true
    [HttpGet("all")]
    public async Task<IActionResult> GetAllLevels([FromQuery] bool asc = true)
    {
        try
        {
            var response = await _levelAssignService.GetLevels(asc);

            return Ok(ApiResponse<IEnumerable<LevelModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<LevelModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }    
    
    // Route: /api/level/points
    [HttpPost("points")]
    public async Task<IActionResult> GetLevelByPoints([FromBody] LevelRequestModel requestModel)
    {
        try
        {
            var response = await _levelAssignService.GetLevelByPointsAsync(requestModel);

            return Ok(ApiResponse<string>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<string>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/level/points/next-level
    [HttpPost("points/next-level")]
    public async Task<IActionResult> GetNextLevelByPoints([FromBody] LevelRequestModel requestModel)
    {
        try
        {
            var response = await _levelAssignService.GetNextLevelByPointsAsync(requestModel);

            return Ok(ApiResponse<string>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<string>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // // Route: /api/level/points
    // [HttpGet("points")]
    // public async Task<IActionResult> GetLevelByPoints([FromBody] LevelRequestModel requestModel)
    // {
    //     try
    //     {
    //         var userId = await GetUserIdFromIdentity();
    //         
    //         // var response = await _trophyService.JoinTrophyChallengeByIdAsync(userId, trophyId);
    //
    //         return Ok(ApiResponse<bool>.Success(response));
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(ApiResponse<bool>
    //             .Fail(new List<ApiValidationError> { new(null, e.Message) }));
    //     }
    // }
    
    private async Task<int> GetUserIdFromIdentity()
    {
        var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
            .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", authorizationHeaderValue);
        var userIdResponse = await _httpClient.GetAsync(UserIdApiUrl);
        userIdResponse.EnsureSuccessStatusCode();
        
        var jsonResponse = await userIdResponse.Content.ReadAsStringAsync();
        var userId = Utilities.GetJsonPropertyAsInteger(jsonResponse, new[] { "result" });
        return userId;
    }
}
