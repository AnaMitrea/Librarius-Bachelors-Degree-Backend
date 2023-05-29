using Library.DataAccess.DTOs.User;
using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IUserRepository
{
    Task<bool> RegisterAsLibraryUser(int id, string username);
    
    public Task<User> GetUserById(int id);

    public Task<bool> CheckUserIsSubscribedAsync(string username, int authorId);
    
    public Task<bool> SetUserSubscribed(string username, int authorId);
    
    public Task<bool> SetUserUnsubscribed(string username, int authorId);
    public Task<int> GetUserMinutesLoggedAsync(string username);
    public Task<IEnumerable<UserLeaderboardByMinutesDto>> GetAllUsersMinutesLoggedAsync();
    public Task<IEnumerable<UserLeaderboardByBooksDto>> GetAllUsersByNumberOfBooksDescAsync();
}