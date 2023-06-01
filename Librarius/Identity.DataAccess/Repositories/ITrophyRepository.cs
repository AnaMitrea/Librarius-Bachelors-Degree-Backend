using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Repositories;

public interface ITrophyRepository
{
    Task<IEnumerable<Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit = false);

    Task<Dictionary<string, IEnumerable<Trophy>>> GetUserAllCompletedTrophiesAsync(string username);
    
    Task<IEnumerable<Trophy>> GetUserCompletedTrophiesByCategoryAsync(string username, string category);
    
    Task<Dictionary<string, IEnumerable<Trophy>>> GetUserInProgressTrophiesAsync(string username);
    
    Task<IEnumerable<Trophy>> GetUserInProgressTrophiesByCategoryAsync(string username, string category);
    Task<bool> JoinTrophyChallengeByIdAsync(string username, int trophyId);
    Task<bool> LeaveTrophyChallengeByIdAsync(string username, int trophyId);
}