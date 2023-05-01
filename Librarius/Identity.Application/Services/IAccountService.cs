using Identity.Application.Models.Requests;
using Identity.Application.Models.User;

namespace Identity.Application.Services;

public interface IAccountService
{
    Task<UserAccountModel> CreateAccountAsync(RegisterRequestModel registerRequest);
    
    Task<UserAccountModel?> GetAccountAsync(string username, string password);

    Task<UserModel?> GetUserInformationAsync(string username);

    void CheckUsernameExistence(string username);

    void CheckEmailExistence(string email);
}