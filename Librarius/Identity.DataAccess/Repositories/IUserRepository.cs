using Identity.DataAccess.DTOs;

namespace Identity.DataAccess.Repositories;

public interface IUserRepository
{
    public Task<IEnumerable<UserLeaderboardByPointsDto>> GetAllUsersByPointsDescAsync();
    Task<IEnumerable<string>> GetUserDashboardActivityAsync(string username);
    Task<int> FindUserIdByUsernameAsync(string username);
    Task<int> AddPointsToUserAsync(string username, int points);
    
    Task<string> SetUserLevelAsync(string username, string level);
}