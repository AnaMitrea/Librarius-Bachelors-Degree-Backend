using Library.DataAccess.DTOs.User;
using Library.DataAccess.Entities.BookRelated;
using Library.DataAccess.Entities.Library;
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
        if (user == default) throw new UnauthorizedAccessException();

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
        if (user == default) throw new UnauthorizedAccessException();
        
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

    // Get total reading time
    public async Task<int> GetUserTotalReadingTimeAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == default) throw new UnauthorizedAccessException();
        
        var totalMinutesLogged = await _dbContext.UserReadingBooks
            .Where(ur => ur.UserId == user.Id)
            .SumAsync(ur => ur.MinutesSpent);

        return totalMinutesLogged == 0 ? 0 : totalMinutesLogged;
    }

    public async Task<int> GetUserTotalCompletedBooksAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == default) throw new UnauthorizedAccessException();
    
        var totalUserCompletedBooks = await _dbContext.UserReadingBooks
            .CountAsync(ur => ur.UserId == user.Id && ur.IsBookFinished);

        return totalUserCompletedBooks;
    }

    public async Task<IEnumerable<UserLeaderboardByMinutesDto>> GetAllUsersByReadingTimeDescAsync()
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
        if (user == null) throw new UnauthorizedAccessException();
        
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

    public async Task<IEnumerable<Book>> GetReadingBooksInProgressUserAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null) throw new UnauthorizedAccessException();
        
        await _dbContext.Entry(user)
            .Collection(u => u.ReadingBooks)
            .Query()
            .Include(rb => rb.Book)
            .ThenInclude(b => b.Author)
            .LoadAsync();
        
        var inProgressBooks = user.ReadingBooks
            .Where(rb => !rb.IsBookFinished)
            .Select(rb => rb.Book);
    
        return inProgressBooks;
    }

    public async Task<IEnumerable<Book>> GetUserFavoriteBooksAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        await _dbContext.Entry(user)
            .Collection(u => u.FavoriteBooks)
            .Query()
            .Include(fb => fb.Book)
            .ThenInclude(b => b.Author)
            .LoadAsync();

        var favoriteBooks = user.FavoriteBooks.Select(fb => fb.Book);

        return favoriteBooks;
    }

    public async Task DeleteUserFavoriteBookByIdASync(string username, int bookId)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null) throw new UnauthorizedAccessException();

        var book = await _dbContext.UserFavoriteBooks
            .SingleOrDefaultAsync(b => b.User.Username == username && b.BookId == bookId);
        if (book == null) throw new Exception("Book not found");

        _dbContext.UserFavoriteBooks.Remove(book);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Author>> GetUserAuthorsSubscriptionsAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null) throw new UnauthorizedAccessException();

        await _dbContext.Entry(user)
            .Collection(u => u.Subscriptions)
            .Query()
            .Include(sb => sb.Author)
            .LoadAsync();

        var subscribedAuthors = user.Subscriptions.Select(sb => sb.Author);

        return subscribedAuthors;
    }

    public async Task RemoveUserSubscribedAuthorByIdAsync(string username, int authorId)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null) throw new UnauthorizedAccessException();

        var subscription = await _dbContext.UserAuthorSubscriptions
            .SingleOrDefaultAsync(sb => sb.User.Username == username && sb.AuthorId == authorId);
        if (subscription == null) throw new Exception("Subscription not found");

        _dbContext.UserAuthorSubscriptions.Remove(subscription);

        await _dbContext.SaveChangesAsync();
    }
}