using Library.API.Models;
using Library.API.Utils;
using Library.Application.Models.Book;
using Library.Application.Models.Book.Author;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Reading.Response;
using Library.Application.Models.Book.Trending;
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
    private readonly IBookService _bookService;
    private readonly ITriggerRewardService _triggerRewardService;

    public LibraryUserController(
        IUserService userService, 
        ITriggerRewardService triggerRewardService, 
        IBookService bookService)
    {
        _userService = userService;
        _triggerRewardService = triggerRewardService;
        _bookService = bookService;
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
    
    // Check if user finished Book
    // Route: /api/library/user/book/{bookId}/check-reading-completed
    [HttpGet("book/{bookId:int}/check-reading-completed")]
    public async Task<IActionResult> CheckFinishedReadingBookByIdAsync(int bookId)
    {
        try
        {
            var username = GetUsernameFromToken();
            
            var response = await _bookService.CheckIsBookFinishedReading(bookId, username);

            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // User Reading Time for Book
    // Route: /api/library/user/book/time-spent
    [HttpPost("book/time-spent")]
    public async Task<IActionResult> GetUserReadingTimeForBookIdSpent(ReadingRequestModel requestModel)
    {
        try
        {
            var username = GetUsernameFromToken();
            
            var response = await _bookService.GetUserReadingTimeSpentAsync(requestModel, username);

            return Ok(ApiResponse<ReadingTimeSpentResponseModel>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<ReadingTimeSpentResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }

    // Update User Reading Time
    // Route: /api/library/user/book/time-spent/update
    [HttpPut("book/time-spent/update")]
    public async Task<IActionResult> UpdateUserReadingTimeForBookIdSpent(UserReadingBookRequestModel requestModel)
    {
        try
        {
            var username = GetUsernameFromToken();
            
            var totalMinutesForBookId = await _bookService.UpdateUserReadingTimeSpentAsync(requestModel, username);

            return Ok(ApiResponse<bool>.Success(true));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Finish Reading Book
    // Route: /api/library/user/book/complete-reading
    [HttpPost("book/complete-reading")]
    public async Task<IActionResult> SetFinishedReadingBookByIdAsync(UserReadingBookRequestModel requestModel)
    {
        try
        {
            var username = GetUsernameFromToken();
            var response = await _bookService.SetFinishedReadingBookByIdAsync(requestModel, username);
            
            var totalReadingTime = await _userService.GetUserTotalReadingTimeAsync(username);
            
            await _triggerRewardService.TriggerUpdateTotalReadingTime(
                totalReadingTime, 
                false,
                GetAuthorizationHeaderValue()
            );
            
            var totalReadingBooks = await _userService.GetUserTotalCompletedBooksAsync(username);
            await _triggerRewardService.TriggerUpdateTotalReadingBooks(
                totalReadingBooks, 
                true,
                GetAuthorizationHeaderValue()
            );

            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/user/book/reading/in-progress
    [HttpGet("book/reading/in-progress")]
    public async Task<IActionResult> GetReadingBooksInProgressUser()
    {
        try
        {
            var username = GetUsernameFromToken();
            
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
    
    // Total Reading Time
    // Route: /api/library/user/minutes-logged
    [HttpGet("minutes-logged")]
    public async Task<IActionResult> GeUserTotalReadingTime()
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _userService.GetUserTotalReadingTimeAsync(username);

            return Ok(ApiResponse<int>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<int>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
    
    // Route: /api/library/user/all/minutes-logged
    [HttpGet("all/minutes-logged")]
    public async Task<IActionResult> GetAllUsersByTotalReadingTimeDesc()
    {
        try
        {
            var response = await _userService.GetAllUsersByReadingTimeDescAsync();

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
            var username = GetUsernameFromToken();
            
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

    // Route: /api/library/user/favorite/books
    [HttpGet("favorite/books")]
    public async Task<IActionResult> GetUserFavoriteBooks()
    {
        try
        {
            var username = GetUsernameFromToken();
            
            var response = await _userService.GetUserFavoriteBooksAsync(username);
            return Ok(ApiResponse<IEnumerable<BookMinimalResponseModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<BookMinimalResponseModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/library/user/favorite/{bookId:int}/remove
    [HttpDelete("favorite/{bookId:int}/remove")]
    public async Task<IActionResult> DeleteUserFavoriteBookById(int bookId)
    {
        try
        {
            var username = GetUsernameFromToken();
            
            await _userService.DeleteUserFavoriteBookByIdASync(username, bookId);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<BookMinimalResponseModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/library/user/authors
    [HttpGet("authors")]
    public async Task<IActionResult> GetUserSubscribedAuthors()
    {
        try
        {
            var username = GetUsernameFromToken();
            
            var response = await _userService.GetUserAuthorsSubscriptionsAsync(username);
            return Ok(ApiResponse<IEnumerable<AuthorResponseModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<IEnumerable<AuthorResponseModel>>
                .Fail(new List<ApiValidationError> { new(null, e.Message) }));
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