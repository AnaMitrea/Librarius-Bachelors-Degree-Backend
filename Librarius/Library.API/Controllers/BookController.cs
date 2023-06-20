﻿using System.Net.Http.Headers;
using Library.API.Models;
using Library.API.Utils;
using Library.Application.Models.Book;
using Library.Application.Models.Book.Explore.Bookshelf;
using Library.Application.Models.Book.Explore.Category;
using Library.Application.Models.Book.Favorite;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Reading.Response;
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
    private readonly ITriggerRewardService _triggerRewardService;

    public BookController(IBookService bookService, IReviewService reviewService, ITriggerRewardService triggerRewardService)
    {
        _bookService = bookService;
        _reviewService = reviewService;
        _triggerRewardService = triggerRewardService;
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
    
    // Route: /api/library/book/{bookId}/categoryId
    [HttpGet("{bookId:int}/categoryId")]
    public async Task<IActionResult> GetCategoryIdOfBookById(int bookId)
    {
        try
        {
            var response = await _bookService.GetCategoryIdOfBookByIdAsync(bookId);

            return Ok(ApiResponse<int>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<int>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/book/reviews
    [HttpPost("reviews")]
    public async Task<IActionResult> PostReviewsByBookIdAsync(ReviewRequestModel reviewRequestModel)
    {
        try
        {
            var username = GetUsernameFromToken();
            
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
            var username = GetUsernameFromToken();
            
            var response = await _reviewService.SetUserReviewByBookIdAsync(requestModel, username);

            var hasWonAny = false;
            if (requestModel.ReviewContent.Length >= 1500)
            {
                hasWonAny = await _triggerRewardService.TriggerRewardForLengthyReview(GetAuthorizationHeaderValue());
            }
            
            return Ok(ApiResponse<bool>.Success(hasWonAny));
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
            var username = GetUsernameFromToken();
            
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

    // Route: /api/library/book/bookshelves?maxResults=...?title=...?books=true
    [HttpGet("bookshelves")]
    public async Task<IActionResult> GetBooksGroupedByBookshelf(
        [FromQuery] bool books,
        [FromQuery] int? maxResults = null,
        [FromQuery] string? title = null
    )
    {
        try
        {
            if (books)
            {
                var response = await _bookService.GetBooksGroupedByBookshelf(maxResults, title);
                return Ok(ApiResponse<Dictionary<string, BooksForBookshelfResponseModel>>.Success(response));
            }
            else
            {
                var response = await _bookService.GetGroupedBookshelves(title);
                return Ok(ApiResponse<Dictionary<string, BooksForBookshelfResponseModel>>.Success(response));
            }
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<Dictionary<string, BooksForBookshelfResponseModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/book/ordered/bookshelves?maxResults=...?title=...
    [HttpGet("ordered/bookshelves")]
    public async Task<IActionResult> GetOrderedBooksGroupedByBookshelf(
        [FromQuery] int? maxResults = null,
        [FromQuery] string? title = null
    )
    {
        try
        {
            var response = await _bookService.GetOrderedBooksGroupedByBookshelf(maxResults, title);
            return Ok(ApiResponse<Dictionary<string, OrderedBooksForBookshelfResponseModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<Dictionary<string, OrderedBooksForBookshelfResponseModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/book/categories?maxResults=...&books=...&title=...
    [HttpGet("categories")]
    public async Task<IActionResult> GetBooksGroupedByCategories(
        [FromQuery] bool books,
        [FromQuery] int? maxResults = null,
        [FromQuery] string? title = null
    )
    {
        try
        {
            if (books)
            {
                var response = 
                    await _bookService.GetBooksGroupedByCategoryAndBookshelf(maxResults, title);
                return Ok(ApiResponse<List<BooksForCategoryResponseModel>>.Success(response));
            }
            else
            {
                var response = 
                    await _bookService.GetGroupedCategoryAndBookshelf(title);
                return Ok(ApiResponse<List<BooksForCategoryResponseModel>>.Success(response));
            }
           
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<object>.Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/library/book/ordered/categories?startFrom=...&bookshelfTitle=...&categoryTitle=...&maxResults=...
    [HttpGet("ordered/categories")]
    public async Task<IActionResult> GetOrderedBooksGroupedByCategories(
        [FromQuery] string startFrom,
        [FromQuery] string bookshelfTitle,
        [FromQuery] string categoryTitle,
        [FromQuery] int? maxResults = null
    )
    {
        try
        {
            var response = await _bookService.GetOrderedBooksGroupedByCategories
                (startFrom, bookshelfTitle, categoryTitle, maxResults);
            return Ok(ApiResponse<List<OrderedBookshelfCategoryBooksResponseModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<List<OrderedBookshelfCategoryBooksResponseModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/book/favorite/toggle
    [HttpPost("favorite/toggle")]
    public async Task<IActionResult> SetOrRemoveFavoriteBook([FromBody] BookFavoriteRequestModel requestModel)
    {
        try
        {
            var response = await _bookService.SetOrRemoveFavoriteBookAsync(
                requestModel,
                GetUsernameFromToken()
            );
            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }));
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