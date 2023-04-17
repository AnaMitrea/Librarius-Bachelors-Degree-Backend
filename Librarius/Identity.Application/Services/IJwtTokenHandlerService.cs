using Identity.Application.Models;
using Identity.Application.Models.Requests;

namespace Identity.Application.Services;

public interface IJwtTokenHandlerService
{
    Task<AuthenticationResponseModel?> AuthenticateAccount(AuthenticationRequestModel authRequest);
}