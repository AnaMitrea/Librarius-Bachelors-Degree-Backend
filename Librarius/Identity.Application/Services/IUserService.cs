using Identity.Application.Models.User;

namespace Identity.Application.Services;

public interface IUserService
{
    public Task<IEnumerable<UserLeaderboardByPoints>> GetAllUsersByPointsDescAsync();
    Task<IEnumerable<string>> GetUserDashboardActivityAsync(string username);
}