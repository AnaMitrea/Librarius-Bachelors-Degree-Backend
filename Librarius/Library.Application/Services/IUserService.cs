using Library.Application.Models.LibraryUser;

namespace Library.Application.Services;

public interface IUserService
{
    public Task<bool> RegisterAsLibraryUser(RegisterUserRequestModel requestModel);
    
    public Task<bool> CheckUserIsSubscribedAsync(string username, int authorId);
    
    public Task<bool> SetUserSubscribed(string username, int authorId);
    
    public Task<bool> SetUserUnsubscribed(string username, int authorId);
}