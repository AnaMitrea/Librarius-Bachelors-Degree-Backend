using Identity.Application.Models.Requests;
using Identity.Application.Models.User;

namespace Identity.Application.Services;

public interface IAccountService
{
    Task<AuthenticationResponseModel?> UpdateUserActivity(string username);
    
    Task<RegisterUserAccountModel> CreateAccountAsync(RegisterRequestModel registerRequest);

    Task<bool> DeleteAccountAsync(int userId);
    
    Task<UserAccountModel?> GetAccountAsync(string username, string password);

    Task<DashboardUserModel?> GetUserInformationAsync(string username);
    
    Task<UserEmailResponse> GetUserEmailAndUsernameAsync(string username);

    Task<bool> CheckUsernameExistence(string username);

    Task<bool> CheckEmailExistence(string email);
    
}