using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IUserRepository
{
    public Task<User> GetUserById(int id);

    public Task<bool> CheckUserIsSubscribedAsync(string username, int authorId);
    
    public Task<bool> SetUserSubscribed(string username, int authorId);
    
    public Task<bool> SetUserUnsubscribed(string username, int authorId);
}