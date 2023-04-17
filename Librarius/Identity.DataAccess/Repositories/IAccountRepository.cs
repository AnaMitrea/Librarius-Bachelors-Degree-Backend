using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetAccountAsync(string username, string password);
}