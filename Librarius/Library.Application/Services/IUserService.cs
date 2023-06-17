using Library.Application.Models.Book;
using Library.Application.Models.Book.Author;
using Library.Application.Models.LibraryUser.Request;
using Library.Application.Models.LibraryUser.Response;

namespace Library.Application.Services;

public interface IUserService
{
    public Task<bool> RegisterAsLibraryUser(RegisterUserRequestModel requestModel);
    
    public Task<bool> CheckUserIsSubscribedAsync(string username, int authorId);
    
    public Task<bool> SetUserSubscribed(string username, int authorId);
    
    public Task<bool> SetUserUnsubscribed(string username, int authorId);
    
    public Task<int> GetUserTotalReadingTimeAsync(string username);
    
    public Task<int> GetUserTotalCompletedBooksAsync(string username);
    
    public Task<IEnumerable<UserLeaderboardByMinutes>> GetAllUsersByReadingTimeDescAsync();
    
    public Task<IEnumerable<UserLeaderboardByBooks>> GetAllUsersByNumberOfBooksDescAsync();
    
    public Task<IEnumerable<UserReadingFeed>> GetUserForReadingFeedAsync();
    
    public Task<Dictionary<int, UserBookReadingTimeTrackerResponse>> GetBookTimeReadingTrackersByUserAsync(
        string username);

    public Task<IEnumerable<BookMinimalResponseModel>> GetReadingBooksInProgressUserAsync(string username);
    
    public Task<IEnumerable<BookMinimalResponseModel>> GetUserFavoriteBooksAsync(string username);
    public Task DeleteUserFavoriteBookByIdASync(string username, int bookId);
    public Task<IEnumerable<AuthorResponseModel>> GetUserAuthorsSubscriptionsAsync(string username);
    public Task RemoveUserSubscribedAuthorByIdAsync(string username, int authorId);
}