using Identity.DataAccess.DTOs;

namespace Identity.DataAccess.Repositories;

public interface IUserRepository
{
    public Task<IEnumerable<UserLeaderboardByPointsDto>> GetAllUsersByPointsDescAsync();
    Task<IEnumerable<string>> GetUserDashboardActivityAsync(string username);
    Task<int> FindUserIdByUsernameAsync(string username);
}