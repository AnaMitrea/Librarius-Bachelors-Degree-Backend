using Identity.Application.Models.User;

namespace Identity.Application.Services;

public interface IAccountService
{
    Task<UserAccountModel?> GetAccountAsync(string username, string password);
}