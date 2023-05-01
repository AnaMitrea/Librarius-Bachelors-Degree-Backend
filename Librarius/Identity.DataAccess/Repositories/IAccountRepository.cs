using Identity.DataAccess.DTOs;
using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Repositories;

public interface IAccountRepository
{
    Task<bool> FindAccountByUsernameAsync(string username);
    
    Task<Account?> GetAccountAsync(string username, string password);
    
    Task<Account?> GetUserInformationAsync(string username);

    Task<Account?> UpdateUserInformationAsync(Account userModel);
}