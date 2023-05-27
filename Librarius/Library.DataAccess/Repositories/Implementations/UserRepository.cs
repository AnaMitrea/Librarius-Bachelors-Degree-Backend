using Library.DataAccess.Entities;
using Library.DataAccess.Entities.BookRelated;
using Library.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _dbContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    public async Task<bool> RegisterAsLibraryUser(int id, string username)
    {
        if (await _dbContext.Users.FindAsync(id) != null) throw new Exception("Id duplicated");
        if (await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username) != null) throw new Exception("User already registered.");

        var newUser = new User
        {
            Id = id,
            Username = username
        };
        
        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == default) throw new Exception("User not found");

        return user;
    }

    public async Task<bool> CheckUserIsSubscribedAsync(string username, int authorId)
    {
        var subscription = await _dbContext.UserAuthorSubscriptions
            .SingleOrDefaultAsync(s => s.User.Username == username && s.Author.Id == authorId);

        return subscription != null;
    }

    public async Task<bool> SetUserSubscribed(string username, int authorId)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == default) throw new Exception("User not found");
        
        var subscription = new Subscription
        {
            UserId = user.Id,
            AuthorId = authorId
        };
        
        _dbContext.UserAuthorSubscriptions.Add(subscription);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SetUserUnsubscribed(string username, int authorId)
    {
        var subscription = await _dbContext.UserAuthorSubscriptions
            .SingleOrDefaultAsync(s => s.User.Username == username && s.Author.Id == authorId);

        if (subscription == default) throw new Exception("User is not subscribed to author.");

        _dbContext.UserAuthorSubscriptions.Remove(subscription);
        await _dbContext.SaveChangesAsync();

        return false;
    }
}