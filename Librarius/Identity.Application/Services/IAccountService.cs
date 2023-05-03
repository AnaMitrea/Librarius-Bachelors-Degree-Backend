using Identity.Application.Models.Requests;
using Identity.Application.Models.User;

namespace Identity.Application.Services;

public interface IAccountService
{
    Task<UserAccountModel> CreateAccountAsync(RegisterRequestModel registerRequest);
    
    Task<UserAccountModel?> GetAccountAsync(string username, string password);

    Task<DashboardUserModel?> GetUserInformationAsync(string username);

    Task<bool> CheckUsernameExistence(string username);

    Task<bool> CheckEmailExistence(string email);
}