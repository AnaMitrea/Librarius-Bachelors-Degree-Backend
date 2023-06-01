using Library.Application.Models.Book;
using Library.Application.Models.LibraryUser.Request;
using Library.Application.Models.LibraryUser.Response;

namespace Library.Application.Services;

public interface IUserService
{
    public Task<bool> RegisterAsLibraryUser(RegisterUserRequestModel requestModel);
    
    public Task<bool> CheckUserIsSubscribedAsync(string username, int authorId);
    
    public Task<bool> SetUserSubscribed(string username, int authorId);
    
    public Task<bool> SetUserUnsubscribed(string username, int authorId);
    
    public Task<int> GetUserMinutesLoggedAsync(string username);
    
    public Task<IEnumerable<UserLeaderboardByMinutes>> GetAllUsersByMinutesLoggedDescAsync();
    
    public Task<IEnumerable<UserLeaderboardByBooks>> GetAllUsersByNumberOfBooksDescAsync();
    
    public Task<IEnumerable<UserReadingFeed>> GetUserForReadingFeedAsync();
    
    public Task<Dictionary<int, UserBookReadingTimeTrackerResponse>> GetBookTimeReadingTrackersByUserAsync(
        string username);

    Task<IEnumerable<BookMinimalResponseModel>> GetReadingBooksInProgressUserAsync(string username);
}