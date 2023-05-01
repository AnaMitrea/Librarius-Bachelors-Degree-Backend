using Identity.Application.Models.Requests;

namespace Identity.Application.Services;

public interface IAccountActivityService
{
    Task<AuthenticationResponseModel?> UpdateUserActivity(string username);
}