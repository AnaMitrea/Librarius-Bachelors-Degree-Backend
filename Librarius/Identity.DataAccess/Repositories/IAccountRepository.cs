using Identity.DataAccess.DTOs;
using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Repositories;

public interface IAccountRepository
{
    Task<Account> CreateAccountAsync(RegisterUserModel registerUser);
    
    void CheckUsernameExistence(string username);

    void CheckEmailExistence(string email);

    Task<bool> FindAccountByUsernameAsync(string username);
    
    Task<Account?> GetAccountAsync(string username, string password);
    
    Task<Account?> GetUserInformationAsync(string username);

    Task<Account?> UpdateUserInformationAsync(Account userModel);
}