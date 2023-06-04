namespace Trophy.DataAccess.Repositories;

public interface ITrophyRepository
{
    Task<IEnumerable<Entities.Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit = false);

    Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserAllCompletedTrophiesAsync(int userId);
    
    Task<IEnumerable<Entities.Trophy>> GetUserCompletedTrophiesByCategoryAsync(int userId, string category);
    
    Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserInProgressTrophiesAsync(int userId);
    
    Task<IEnumerable<Entities.Trophy>> GetUserInProgressTrophiesByCategoryAsync(int userId, string category);
    Task<bool> JoinTrophyChallengeByIdAsync(int userId, int trophyId);
    Task<bool> LeaveTrophyChallengeByIdAsync(int userId, int trophyId);
}