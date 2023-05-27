using System.Net.Http.Headers;
using Library.API.Models;
using Library.API.Utils;
using Library.Application.Models.Book.Author;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Library.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;
    private readonly IUserService _userService;
    private readonly HttpClient _httpClient;

    public AuthorController(IAuthorService authorService, IUserService userService, HttpClient httpClient)
    {
        _authorService = authorService;
        _userService = userService;
        _httpClient = httpClient;
    }
    
    // Route: /api/library/author/{authorId}
    [HttpGet("{authorId:int}")]
    public async Task<IActionResult> GetAuthorInformationAsync(int authorId)
    {
        try
        {
            var response = await _authorService.GetAuthorInformationByIdAsync(authorId);

            return Ok(ApiResponse<AuthorResponseModel>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<AuthorResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/author/{authorId}/subscription
    [HttpPost("{authorId:int}/subscription")]
    public async Task<IActionResult> SetAuthorSubscription(int authorId)
    {
        var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
            .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
        var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
        
        try
        {
            var isUserSubscribed = await _userService.CheckUserIsSubscribedAsync(username, authorId);

            if (isUserSubscribed) 
            {
                // Unsubscribe
                var response = await _userService.SetUserUnsubscribed(username, authorId);
                return Ok(ApiResponse<bool>.Success(response));
            }
            else
            {
                // Subscribe
                var response = await _userService.SetUserSubscribed(username, authorId);
                
                var emailApiUrl = $"http://localhost:5164/api/email/{authorId}/subscription";
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", authorizationHeaderValue);
                
                var emailApiResponse = await _httpClient.PostAsync(emailApiUrl, null);
                emailApiResponse.EnsureSuccessStatusCode();
                
                return Ok(ApiResponse<bool>.Success(response));
            }
        }
        catch (Exception e)
        {
            // Rollback: Delete the subscription from the table
            try
            {
                await _userService.SetUserUnsubscribed(username, authorId);
            }
            catch (Exception rollbackException)
            {
                return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, rollbackException.Message) }));
            }

            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }));
        }
    }
    
    // Route: /api/library/author/{authorId}/subscription/status
    [HttpGet("{authorId:int}/subscription/status")]
    public async Task<IActionResult> GetAuthorSubscriptionStatus(int authorId)
    {
        try
        {
            var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
            
            var response = await _userService.CheckUserIsSubscribedAsync(username, authorId);

            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/author/materials
    [HttpPost("materials")]
    public async Task<IActionResult> GetBooksAsync(MaterialRequestModel requestModel)
    {
        try
        {
            var response = await _authorService.GetAuthorBooksAsync(requestModel);

            return Ok(ApiResponse<ICollection<MaterialsResponseModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<ICollection<MaterialsResponseModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
}