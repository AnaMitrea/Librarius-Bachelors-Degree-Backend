using AutoMapper;
using Library.Application.Models.Book;
using Library.Application.Models.Book.Author;
using Library.Application.Models.LibraryUser.Request;
using Library.Application.Models.LibraryUser.Response;
using Library.DataAccess.Repositories;

namespace Library.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool> RegisterAsLibraryUser(RegisterUserRequestModel requestModel)
    {
        return await _userRepository.RegisterAsLibraryUser(requestModel.Id, requestModel.Username);
    }

    public async Task<bool> CheckUserIsSubscribedAsync(string username, int authorId)
    {
        return await _userRepository.CheckUserIsSubscribedAsync(username, authorId);
    }

    public async Task<bool> SetUserSubscribed(string username, int authorId)
    {
        return await _userRepository.SetUserSubscribed(username, authorId);
    }

    public async Task<bool> SetUserUnsubscribed(string username, int authorId)
    {
        return await _userRepository.SetUserUnsubscribed(username, authorId);
    }

    public async Task<int> GetUserTotalReadingTimeAsync(string username)
    {
        return await _userRepository.GetUserTotalReadingTimeAsync(username);
    }

    public async Task<int> GetUserTotalCompletedBooksAsync(string username)
    {
        return await _userRepository.GetUserTotalCompletedBooksAsync(username);
    }

    public async Task<IEnumerable<UserLeaderboardByMinutes>> GetAllUsersByReadingTimeDescAsync()
    {
        var response = await _userRepository.GetAllUsersByReadingTimeDescAsync();
        
        return _mapper.Map<IEnumerable<UserLeaderboardByMinutes>>(response);
    }

    public async Task<IEnumerable<UserLeaderboardByBooks>> GetAllUsersByNumberOfBooksDescAsync()
    {
        var response = await _userRepository.GetAllUsersByNumberOfBooksDescAsync();
        
        return _mapper.Map<IEnumerable<UserLeaderboardByBooks>>(response);
    }

    public async Task<IEnumerable<UserReadingFeed>> GetUserForReadingFeedAsync()
    {
        var response = await _userRepository.GetUserForReadingFeedAsync();
        
        return _mapper.Map<IEnumerable<UserReadingFeed>>(response);
    }

    public async Task<Dictionary<int, UserBookReadingTimeTrackerResponse>>
        GetBookTimeReadingTrackersByUserAsync(string username)
    {
        var response = await _userRepository.GetBookTimeReadingTrackersByUserAsync(username);
        
        return _mapper.Map<Dictionary<int, UserBookReadingTimeTrackerResponse>>(response);
    }

    public async Task<IEnumerable<BookMinimalResponseModel>> GetReadingBooksInProgressUserAsync(string username)
    {
        var response = await _userRepository.GetReadingBooksInProgressUserAsync(username);
        
        return _mapper.Map<IEnumerable<BookMinimalResponseModel>>(response);
    }

    public async Task<IEnumerable<BookMinimalResponseModel>> GetUserFavoriteBooksAsync(string username)
    {
        var response = await _userRepository.GetUserFavoriteBooksAsync(username);
        
        return _mapper.Map<IEnumerable<BookMinimalResponseModel>>(response);
    }

    public async Task DeleteUserFavoriteBookByIdASync(string username, int bookId)
    {
        await _userRepository.DeleteUserFavoriteBookByIdASync(username, bookId);
    }

    public async Task<IEnumerable<AuthorResponseModel>> GetUserAuthorsSubscriptionsAsync(string username)
    {
        var response = await _userRepository.GetUserAuthorsSubscriptionsAsync(username);
        
        return _mapper.Map<IEnumerable<AuthorResponseModel>>(response);
    }
}