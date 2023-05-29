using Library.API.Models;
using Library.API.Utils;
using Library.Application.Models.LibraryUser.Request;
using Library.Application.Models.LibraryUser.Response;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Library.API.Controllers;

[Route("user")]
[ApiController]
public class LibraryUserController : ControllerBase
{
    private readonly IUserService _userService;

    public LibraryUserController(IUserService userService)
    {
        _userService = userService;
    }

    // Route: /api/library/user/register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsLibraryUser([FromBody] RegisterUserRequestModel requestModel)
    {
        try
        {
            var response = await _userService.RegisterAsLibraryUser(requestModel);

            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
    
    // Route: /api/library/user/minutes-logged
    [HttpGet("minutes-logged")]
    public async Task<IActionResult> GetUserMinutesLogged()
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _userService.GetUserMinutesLoggedAsync(username);

            return Ok(ApiResponse<int>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<int>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
    
    // Route: /api/library/user/all/minutes-logged
    [HttpGet("all/minutes-logged")]
    public async Task<IActionResult> GetAllUsersByMinutesLoggedDesc()
    {
        try
        {
            var response = await _userService.GetAllUsersByMinutesLoggedDescAsync();

            return Ok(ApiResponse<IEnumerable<UserLeaderboardByMinutes>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<UserLeaderboardByMinutes>>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
    
    // Route: /api/library/user/all/number-of-books
    [HttpGet("all/number-of-books")]
    public async Task<IActionResult> GetAllUsersByNumberOfBooksDesc()
    {
        try
        {
            var response = await _userService.GetAllUsersByNumberOfBooksDescAsync();

            return Ok(ApiResponse<IEnumerable<UserLeaderboardByBooks>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<UserLeaderboardByBooks>>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
}