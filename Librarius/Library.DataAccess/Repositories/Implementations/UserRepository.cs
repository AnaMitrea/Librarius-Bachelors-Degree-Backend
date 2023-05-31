using Library.DataAccess.DTOs.User;
using Library.DataAccess.Entities;
using Library.DataAccess.Entities.BookRelated;
using Library.DataAccess.Entities.User;
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

    public async Task<int> GetUserMinutesLoggedAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == default)
        {
            throw new Exception("User not found");
        }

        var totalMinutesLogged = await _dbContext.UserReadingBooks
            .Where(ur => ur.UserId == user.Id)
            .SumAsync(ur => ur.MinutesSpent);

        return totalMinutesLogged == 0 ? 0 : totalMinutesLogged;
    }

    public async Task<IEnumerable<UserLeaderboardByMinutesDto>> GetAllUsersMinutesLoggedAsync()
    {
        var userLeaderboard = await _dbContext.Users
            .Join(
                _dbContext.UserReadingBooks,
                user => user.Id,
                userReadingBook => userReadingBook.UserId,
                (user, userReadingBook) => new { user, userReadingBook })
            .GroupBy(x => new { x.user.Id, x.user.Username })
            .Select(x => new UserLeaderboardByMinutesDto
            {
                Id = x.Key.Id,
                Username = x.Key.Username,
                MinutesLogged = x.Sum(ur => ur.userReadingBook.MinutesSpent)
            })
            .OrderByDescending(u => u.MinutesLogged)
            .ToListAsync();

        for (var i = 0; i < userLeaderboard.Count; i++)
        {
            userLeaderboard[i].Position = i + 1;
        }

        return userLeaderboard;
    }

    public async Task<IEnumerable<UserLeaderboardByBooksDto>> GetAllUsersByNumberOfBooksDescAsync()
    {
        var userLeaderboard = await _dbContext.Users
            .Join(
                _dbContext.UserReadingBooks,
                user => user.Id,
                userReadingBook => userReadingBook.UserId,
                (user, userReadingBook) => new { user, userReadingBook })
            .GroupBy(x => new { x.user.Id, x.user.Username })
            .Select(x => new UserLeaderboardByBooksDto
            {
                Id = x.Key.Id,
                Username = x.Key.Username,
                NumberOfBooks = x.Count()
            })
            .OrderByDescending(u => u.NumberOfBooks)
            .ToListAsync();

        for (var i = 0; i < userLeaderboard.Count; i++)
        {
            userLeaderboard[i].Position = i + 1;
        }

        return userLeaderboard;
    }

    public async Task<IEnumerable<UserReadingFeedDto>> GetUserForReadingFeedAsync()
    {
        var users = await _dbContext.UserReadingBooks
            .Where(ur => ur.Timestamp != null & ur.IsBookFinished == true)
            .OrderByDescending(ur => ur.Timestamp)
            .Select(ur => new UserReadingFeedDto
            {
                Id = ur.UserId,
                Username = ur.User.Username,
                Book = ur.Book
            })
            .Take(10)
            .ToListAsync();

        return users;
    }

    public async Task<Dictionary<int, UserBookReadingTracker>> GetBookTimeReadingTrackersByUserAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null) throw new Exception("User not found.");
        
        await _dbContext.Entry(user)
            .Collection(u => u.ReadingBooks)
            .Query()
            .Include(rb => rb.Book)
            .LoadAsync();

        var minutesSpentByBookId = user.ReadingBooks
            .GroupBy(rb => rb.BookId)
            .ToDictionary(group => group.Key,
                group => new UserBookReadingTracker
                {
                    BookId = group.Key,
                    MinutesSpent = group.Sum(rb => rb.MinutesSpent),
                });

        return minutesSpentByBookId;
    }
}