using Identity.API.Models;
using Identity.API.Utils;
using Identity.Application.Models.User;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Identity.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IUserService _userService;

    public UserController(IAccountService accountService, IUserService userService)
    {
        _accountService = accountService;
        _userService = userService;
    }
    
    // Route: /api/user/
    [HttpGet("")]
    public async Task<IActionResult> GetUserInformation()
    {
        var authorizationHeaderValue = HttpContext.Request.Headers[HeaderNames.Authorization]
            .ToString().Replace("bearer ", "");
        var username = Utilities.ExtractUsernameFromAccessToken(authorizationHeaderValue);
        
        try
        {
            var response = await _accountService.GetUserInformationAsync(username);

            if (response == null) throw new Exception("Access Token Invalid.");

            return Ok(ApiResponse<DashboardUserModel>.Success(response));
        }
        catch (Exception e)
        {
            return NotFound(ApiResponse<DashboardUserModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
}