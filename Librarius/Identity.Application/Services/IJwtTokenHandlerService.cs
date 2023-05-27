using Identity.Application.Models.Requests;
using Identity.Application.Models.User;

namespace Identity.Application.Services;

public interface IJwtTokenHandlerService
{
    Task<AuthenticationResponseModel?> AuthenticateAccount(AuthenticationRequestModel authRequest);
    
    Task<RegisterUserAccountModel> RegisterAccount(RegisterRequestModel registerRequest);
}