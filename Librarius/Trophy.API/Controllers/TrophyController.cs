using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Trophy.API.Models;
using Trophy.API.Utils;
using Trophy.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Trophy.Application.Models.Trophy.Request.UserRewardActivity;
using Trophy.Application.Models.Trophy.Response;

namespace Trophy.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TrophyController : ControllerBase
{
    private readonly ITrophyService _trophyService;
    private readonly HttpClient _httpClient;

    private const string UserIdApiUrl = "http://localhost:5164/api/user/id";

    public TrophyController(ITrophyService trophyService, HttpClient httpClient)
    {
        _trophyService = trophyService;
        _httpClient = httpClient;
    }
    
    // Route: /api/trophy/reward/check-win
    [HttpGet("reward/check-win")]
    public async Task<IActionResult> CheckUserIfCanWin()
    {
        try
        {
            var userId = await GetUserIdFromIdentity();
            
            var response = await _trophyService.CheckUserIfCanWinAsync(userId);

            return Ok(ApiResponse<IEnumerable<TrophyModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<TrophyModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/reading-time/reward/update-activity
    [HttpPut("reading-time/reward/update-activity")]
    public async Task<IActionResult> UpdateReadingTimeRewardActivity(
        ReadingTimeUpdateActivityRequestModel requestModel)
    {
        try
        {
            var userId = await GetUserIdFromIdentity();;

            var pointsWon = await _trophyService.UpdateReadingTimeRewardActivityAsync(requestModel, userId);
            
            if (requestModel.CanCheckWin)
            {
                var response = await UpdateUserWithNewLevel(pointsWon);
            }
            
            var hasWonAnyTrophy = pointsWon != 0;
            
            return Ok(ApiResponse<bool>.Success(hasWonAnyTrophy));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<string>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/reading-books/reward/update-activity
    [HttpPut("reading-books/reward/update-activity")]
    public async Task<IActionResult> UpdateReadingBooksRewardActivity(
        ReadingBooksUpdateActivityRequestModel requestModel)
    {
        try
        {
            var userId = await GetUserIdFromIdentity();
            
            var pointsWon = await _trophyService.UpdateReadingBooksRewardActivityAsync(requestModel, userId);

            if (requestModel.CanCheckWin)
            {
                var response = await UpdateUserWithNewLevel(pointsWon);
            }

            var hasWonAnyTrophy = pointsWon != 0;
            
            return Ok(ApiResponse<bool>.Success(hasWonAnyTrophy));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/category-reader/reward/update-activity
    [HttpPut("category-reader/reward/update-activity")]
    public async Task<IActionResult> UpdateCategoryReaderRewardActivity(
        CategoryReaderUpdateActivityRequestModel requestModel)
    {
        try
        {
            var userId = await GetUserIdFromIdentity();
            
            var pointsWon = await _trophyService.UpdateCategoryReaderRewardActivityAsync(requestModel, userId);
            
            if (requestModel.CanCheckWin)
            {
                var response = await UpdateUserWithNewLevel(pointsWon);
            }

            var hasWonAnyTrophy = pointsWon != 0;
            
            return Ok(ApiResponse<bool>.Success(hasWonAnyTrophy));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/activities/reward/update-activity
    [HttpPut("activities/reward/update-activity")]
    public async Task<IActionResult> UpdateActivitiesRewardActivity(ActivitiesUpdateActivityRequestModel requestModel)
    {
        try
        {
            var userId = await GetUserIdFromIdentity();
            
            var pointsWon = await _trophyService.UpdateActivitiesRewardActivityAsync(requestModel, userId);
            
            if (requestModel.CanCheckWin)
            {
                var response = await UpdateUserWithNewLevel(pointsWon);
            }

            var hasWonAnyTrophy = pointsWon != 0;
            
            return Ok(ApiResponse<bool>.Success(hasWonAnyTrophy));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/join/{:trophyId}
    [HttpGet("join/{trophyId:int}")]
    public async Task<IActionResult> JoinTrophyChallengeById([FromRoute] int trophyId)
    {
        try
        {
            var userId = await GetUserIdFromIdentity();
            
            var response = await _trophyService.JoinTrophyChallengeByIdAsync(userId, trophyId);

            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/win/first-login-trophy
    [HttpGet("win/first-login-trophy")]
    public async Task<IActionResult> JoinFirstLoginTrophyChallenge()
    {
        try
        {
            var userId = await GetUserIdFromIdentity();
            
            await _trophyService.JoinTrophyChallengeByIdAsync(userId, 20);

            var requestModel = new ActivitiesUpdateActivityRequestModel
            {
                CanCheckWin = true,
                Criterion = "login"
            };
            
            var pointsWon = await _trophyService.UpdateActivitiesRewardActivityAsync(requestModel, userId);
            await UpdateUserWithNewLevel(pointsWon);

            var hasWonAnyTrophy = pointsWon != 0;
            
            return Ok(ApiResponse<bool>.Success(hasWonAnyTrophy));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/leave/{:trophyId}
    [HttpGet("leave/{trophyId:int}")]
    public async Task<IActionResult> LeaveTrophyChallengeById([FromRoute] int trophyId)
    {
        try
        {
            var userId = await GetUserIdFromIdentity();
            
            var response = await _trophyService.LeaveTrophyChallengeByIdAsync(userId, trophyId);

            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/challenges?category=...&limit=...
    [HttpGet("challenges")]
    public async Task<IActionResult> GetTrophiesByCategory([FromQuery] string category,[FromQuery] bool limit)
    {
        try
        {
            if (string.IsNullOrEmpty(category) || string.IsNullOrWhiteSpace(category))
                throw new Exception("Invalid category parameter.");
            
            var response = await _trophyService.GetTrophiesByCategoryAsync(category, limit);

            return Ok(ApiResponse<IEnumerable<TrophyModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<TrophyModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/user/completed for all completed trophies
    // Route: /api/trophy/user/completed?category=... for all completed trophies for certain category
    [HttpGet("user/completed")]
    public async Task<IActionResult> GetUserCompletedTrophiesAsync([FromQuery] string? category)
    {
        try
        {
            var userId = await GetUserIdFromIdentity();
            
            if (string.IsNullOrEmpty(category) || string.IsNullOrWhiteSpace(category))
            {
                var response = await _trophyService.GetUserAllCompletedTrophiesAsync(userId);
            
                return Ok(ApiResponse<Dictionary<string, IEnumerable<TrophyModel>>>.Success(response));
            }
            else
            {
                var response = await _trophyService.GetUserCompletedTrophiesByCategoryAsync(userId, category);
            
                return Ok(ApiResponse<IEnumerable<TrophyModel>>.Success(response));
            }
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<TrophyModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/trophy/user/in-progress
    // all user IN PROGRESS trophies
    [HttpGet("user/in-progress")]
    public async Task<IActionResult> GetUserInProgressTrophiesAsync([FromQuery] string? category)
    {
        try
        {
            var userId = await GetUserIdFromIdentity();

            if (string.IsNullOrEmpty(category) || string.IsNullOrWhiteSpace(category))
            {
                var response = await _trophyService.GetUserInProgressTrophiesAsync(userId);
            
                return Ok(ApiResponse<Dictionary<string, IEnumerable<TrophyModel>>>.Success(response));
            }
            else
            {
                var response = await _trophyService.GetUserInProgressTrophiesByCategoryAsync(userId, category);
            
                return Ok(ApiResponse<IEnumerable<TrophyModel>>.Success(response));
            }
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<TrophyModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }

    private async Task<string> UpdateUserWithNewLevel(int pointsWon)
    {
        var request = new 
        {
            Points = pointsWon
        };
            
        var requestJson = JsonSerializer.Serialize(request);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAuthorizationHeaderValue());
            
        var httpResponse = await _httpClient.PutAsync(
            "http://localhost:5164/api/user/points/add",
            requestContent
        );
        httpResponse.EnsureSuccessStatusCode();
            
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
        return Utilities.GetJsonPropertyAsString(jsonResponse, new[] { "result" });
    }

    private async Task<int> GetUserIdFromIdentity()
    {
        var authorizationHeaderValue = GetAuthorizationHeaderValue();

        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", authorizationHeaderValue);
        var userIdResponse = await _httpClient.GetAsync(UserIdApiUrl);
        userIdResponse.EnsureSuccessStatusCode();
        
        var jsonResponse = await userIdResponse.Content.ReadAsStringAsync();
        var userId = Utilities.GetJsonPropertyAsInteger(jsonResponse, new[] { "result" });
        return userId;
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