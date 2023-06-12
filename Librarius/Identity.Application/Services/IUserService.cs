using Identity.Application.Models.User;

namespace Identity.Application.Services;

public interface IUserService
{
    public Task<IEnumerable<UserLeaderboardByPoints>> GetAllUsersByPointsDescAsync();
    Task<IEnumerable<string>> GetUserDashboardActivityAsync(string username);
    Task<int> FindUserIdByUsernameAsync(string username);
    Task<int> AddPointsToUserAsync(string username, int points);

    Task<string> SetUserLevelAsync(string username, string level);
}