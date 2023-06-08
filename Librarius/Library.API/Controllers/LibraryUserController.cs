using Library.API.Models;
using Library.API.Utils;
using Library.Application.Models.Book;
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
    private readonly ITriggerRewardService _triggerRewardService;

    public LibraryUserController(IUserService userService, ITriggerRewardService triggerRewardService)
    {
        _userService = userService;
        _triggerRewardService = triggerRewardService;
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
    
    // Route: /api/library/user/reading-feed
    [HttpGet("reading-feed")]
    public async Task<IActionResult> GetUserForReadingFeed()
    {
        try
        {
            var response = await _userService.GetUserForReadingFeedAsync();

            return Ok(ApiResponse<IEnumerable<UserReadingFeed>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<UserReadingFeed>>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

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
    
    // Route: /api/library/user/reading-tracker
    [HttpGet("reading-tracker")]
    public async Task<IActionResult> GetBookTimeReadingTrackersByUser()
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _userService.GetBookTimeReadingTrackersByUserAsync(username);
            return Ok(ApiResponse<Dictionary<int, UserBookReadingTimeTrackerResponse>>
                .Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<Dictionary<int, UserBookReadingTimeTrackerResponse>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
    
    // Route: /api/library/user/reading/in-progress
    [HttpGet("reading/in-progress")]
    public async Task<IActionResult> GetReadingBooksInProgressUser()
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _userService.GetReadingBooksInProgressUserAsync(username);
            return Ok(ApiResponse<IEnumerable<BookMinimalResponseModel>>
                .Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<BookMinimalResponseModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
    
    // Route: /api/library/user/favorite/books
    [HttpGet("favorite/books")]
    public async Task<IActionResult> GetUserFavoriteBooks()
    {
        throw new NotImplementedException();
        // try
        // {
        //     var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
        //         .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
        //     var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
        //     
        //     var response = await _userService.GetBookTimeReadingTrackersByUserAsync(username);
        //     return Ok(ApiResponse<Dictionary<int, UserBookReadingTimeTrackerResponse>>
        //         .Success(response));
        // }
        // catch (Exception e)
        // {
        //     return BadRequest(ApiResponse<Dictionary<int, UserBookReadingTimeTrackerResponse>>
        //         .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        //
        // }
    }
}