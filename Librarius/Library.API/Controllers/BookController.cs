using Library.API.Models;
using Library.API.Utils;
using Library.Application.Models.Book;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Trending;
using Library.Application.Models.Reviews.Request;
using Library.Application.Models.Reviews.Response;
using Library.Application.Services;
using Library.DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Library.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IReviewService _reviewService;

    public BookController(IBookService bookService, IReviewService reviewService)
    {
        _bookService = bookService;
        _reviewService = reviewService;
    }
    
    // Route: /api/library/book/{bookId}
    [HttpGet("{bookId:int}")]
    public async Task<IActionResult> GetBookWithCategoryByIdAsync(int bookId)
    {
        try
        {
            var response = await _bookService.GetBookWithCategoryByIdAsync(bookId);

            return Ok(ApiResponse<BookResponseModel>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<BookResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/book/reviews
    [HttpPost("reviews")]
    public async Task<IActionResult> PostReviewsByBookIdAsync(ReviewRequestModel reviewRequestModel)
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _reviewService.GetReviewsForBookByIdAsync(reviewRequestModel, username);
            
            return Ok(ApiResponse<RatingReviewsResponseModel>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<RatingReviewsResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/book/reviews/submit
    [HttpPost("reviews/submit")]
    public async Task<IActionResult> SetUserReviewByBookIdAsync(UserReviewRequestModel requestModel)
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _reviewService.SetUserReviewByBookIdAsync(requestModel, username);
            
            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/book/reviews/like
    [HttpPut("reviews/like")]
    public async Task<IActionResult> UpdateReviewLikesByBookIdAsync(LikeReviewRequestModel requestModel)
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _reviewService.UpdateLikeStatusAsync(
                username,
                requestModel.ReviewId,
                requestModel.IsLiked
            );
            
            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/book/reviews/{id}/remove
    [HttpDelete("reviews/{reviewId:int}/remove")]
    public async Task<IActionResult> DeleteReviewByIdAsync(int reviewId)
    {
        try
        {
            var response = await _reviewService.DeleteReviewByIdAsync(reviewId);
            
            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Reading Content
    // Route: /api/library/book/read?id=
    [HttpGet("read")]
    public async Task<IActionResult> GetReadingBookByIdAsync([FromQuery] int id)
    {
        try
        {
            var response = await _bookService.GetReadingBookByIdAsync(id);

            return Ok(ApiResponse<BookReadingResponseModel>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<BookReadingResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Reading Content Word count
    // Route: /api/library/book/word-count
    [HttpPost("word-count")]
    public async Task<IActionResult> GetWordCount(ReadingRequestModel requestModel)
    {
        try
        {
            var response = await _bookService.GetBookContentWordCount(requestModel.BookId);

            return Ok(ApiResponse<int>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<int>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Reading Time Content
    // Route: /api/library/book/reading-time
    [HttpPost("reading-time")]
    public async Task<IActionResult> GetReadingTime(ReadingRequestModel requestModel)
    {
        try
        {
            var response = await _bookService.GetReadingTimeOfBookContent(requestModel.BookId);

            return Ok(ApiResponse<ReadingTimeResponseDto>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<ReadingTimeResponseDto>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Check is Book finished
    // Route: /api/library/book/{bookId}/check-reading-completed
    [HttpGet("{bookId:int}/check-reading-completed")]
    public async Task<IActionResult> CheckFinishedReadingBookByIdAsync(int bookId)
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _bookService.CheckIsBookFinishedReading(bookId, username);

            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Finish Reading Content
    // Route: /api/library/book/complete-reading
    [HttpPost("complete-reading")]
    public async Task<IActionResult> SetFinishedReadingBookByIdAsync(CompletedBookRequestModel requestModel)
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _bookService.SetFinishedReadingBookByIdAsync(requestModel, username);

            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }

    /*
     * Route:
     * Trending Now : /api/library/book/trending?duration=now
     * Trending Week : /api/library/book/trending?duration=week
     */
    [HttpGet("trending")]
    public async Task<IActionResult> GetTrendingBooks([FromQuery] string duration)
    {
        try
        {
            switch (duration)
            {
                case "now":
                {
                    // currently trending
                    var response = await _bookService.GetTrendingNowBooksAsync();

                    return Ok(ApiResponse<IEnumerable<BookTrendingResponseModel>>.Success(response));
                }
                case "week":
                {
                    // the top 10 books of the week based on popularity
                    var response = await _bookService.GetTrendingWeekBooksAsync();
            
                    return Ok(ApiResponse<IEnumerable<BookTrendingResponseModel>>.Success(response));
                }
                default:
                    return BadRequest("Invalid parameter.");
            }
        }
        catch (Exception e)
        {
            return BadRequest(
                ApiResponse<IEnumerable<BookTrendingResponseModel>>
                    .Fail(new List<ApiValidationError> { new(null, e.Message) })
            );
        }
    }

    // [HttpGet("bookshelves")]
    // public async Task<IActionResult> GetBooksForAllBookshelves()
    // {
    //     try
    //     {
    //         var response = await _bookService.GetBooksForAllBookshelves();
    //         return Ok(ApiResponse<IEnumerable<ExploreBookResponseModel>>.Success(response));
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(ApiResponse<IEnumerable<ExploreBookResponseModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
    //     }
    //     
    // }
    
    [HttpGet("bookshelves")]
    public async Task<IActionResult> GetBooksGroupedByBookshelf()
    {
        try
        {
            var response = await _bookService.GetBooksGroupedByBookshelf();
            return Ok(ApiResponse<Dictionary<string, List<BookResponseModel>>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<Dictionary<string, List<BookResponseModel>>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
        
    }
}