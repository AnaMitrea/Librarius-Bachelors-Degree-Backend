using Library.DataAccess.DTOs.User;
using Library.DataAccess.Entities.BookRelated;
using Library.DataAccess.Entities.Library;
using Library.DataAccess.Entities.User;

namespace Library.DataAccess.Repositories;

public interface IUserRepository
{
    Task<bool> RegisterAsLibraryUser(int id, string username);
    
    public Task<User> GetUserById(int id);

    public Task<bool> CheckUserIsSubscribedAsync(string username, int authorId);
    
    public Task<bool> SetUserSubscribed(string username, int authorId);
    
    public Task<bool> SetUserUnsubscribed(string username, int authorId);
    public Task<int> GetUserTotalReadingTimeAsync(string username);
    
    public Task<int> GetUserTotalCompletedBooksAsync(string username);
    
    public Task<IEnumerable<UserLeaderboardByMinutesDto>> GetAllUsersByReadingTimeDescAsync();
    public Task<IEnumerable<UserLeaderboardByBooksDto>> GetAllUsersByNumberOfBooksDescAsync();
    public Task<IEnumerable<UserReadingFeedDto>> GetUserForReadingFeedAsync();

    Task<Dictionary<int, UserBookReadingTracker>> GetBookTimeReadingTrackersByUserAsync(string username);
    
    Task<IEnumerable<Book>> GetReadingBooksInProgressUserAsync(string username);
    Task<IEnumerable<Book>> GetUserFavoriteBooksAsync(string username);
    Task DeleteUserFavoriteBookByIdASync(string username, int bookId);
    Task<IEnumerable<Author>> GetUserAuthorsSubscriptionsAsync(string username);
    Task RemoveUserSubscribedAuthorByIdAsync(string username, int authorId);
}