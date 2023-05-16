using System.Collections;
using Library.API.Models;
using Library.Application.Models.Book;
using Library.Application.Models.Reviews;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

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
            var response = await _reviewService.GetReviewsForBookByIdAsync(reviewRequestModel);
            
            return Ok(ApiResponse<RatingReviewsResponseModel>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<RatingReviewsResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
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