using Identity.API.Models;
using Identity.API.Utils;
using Identity.Application.Models.Trophy;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Identity.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TrophyController : ControllerBase
{
    private readonly ITrophyService _trophyService;

    public TrophyController(ITrophyService trophyService)
    {
        _trophyService = trophyService;
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
    
    // Route: /api/trophy/user for all completed trophies
    // Route: /api/trophy/user?category=... for all completed trophies for certain category
    [HttpGet("user")]
    public async Task<IActionResult> GetUserCompletedTrophiesAsync([FromQuery] string? category)
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);

            if (string.IsNullOrEmpty(category) || string.IsNullOrWhiteSpace(category))
            {
                var response = await _trophyService.GetUserAllCompletedTrophiesAsync(username);
            
                return Ok(ApiResponse<ICollection<KeyValuePair<string,IEnumerable<TrophyModel>>>>.Success(response));
            }
            else
            {
                var response = await _trophyService.GetUserCompletedTrophiesByCategoryAsync(username, category);
            
                return Ok(ApiResponse<IEnumerable<TrophyModel>>.Success(response));
            }
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<TrophyModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
}