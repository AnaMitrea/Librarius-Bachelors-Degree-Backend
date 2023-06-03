namespace Trophy.DataAccess.Repositories;

public interface ITrophyRepository
{
    Task<IEnumerable<Entities.Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit = false);

    Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserAllCompletedTrophiesAsync(string username);
    
    Task<IEnumerable<Entities.Trophy>> GetUserCompletedTrophiesByCategoryAsync(string username, string category);
    
    Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserInProgressTrophiesAsync(string username);
    
    Task<IEnumerable<Entities.Trophy>> GetUserInProgressTrophiesByCategoryAsync(string username, string category);
    Task<bool> JoinTrophyChallengeByIdAsync(string username, int trophyId);
    Task<bool> LeaveTrophyChallengeByIdAsync(string username, int trophyId);
}