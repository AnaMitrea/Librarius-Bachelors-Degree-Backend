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
    
    // Route: /api/trophy/user?category=...
    [HttpGet("user")]
    public async Task<IActionResult> GetUserCompletedTrophiesByCategoryAsync([FromQuery] string category)
    {
        try
        {
            if (string.IsNullOrEmpty(category) || string.IsNullOrWhiteSpace(category))
                throw new Exception("Invalid category parameter.");
            
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _trophyService.GetUserCompletedTrophiesByCategoryAsync(username, category);
            
            return Ok(ApiResponse<IEnumerable<TrophyModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<TrophyModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
}