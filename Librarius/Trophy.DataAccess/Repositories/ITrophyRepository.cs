namespace Trophy.DataAccess.Repositories;

public interface ITrophyRepository
{
    Task<IEnumerable<Entities.Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit = false);

    Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserAllCompletedTrophiesAsync(int userId);
    
    Task<IEnumerable<Entities.Trophy>> GetUserCompletedTrophiesByCategoryAsync(int userId, string category);
    
    Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserInProgressTrophiesAsync(int userId);
    
    Task<IEnumerable<Entities.Trophy>> GetUserInProgressTrophiesByCategoryAsync(int userId, string category);
    
    // TROPHY REWARDS
    // -- CHECK --
    Task<bool> CheckUserIfCanWinAsync(int userId);
    
    // -- JOIN --
    Task<bool> JoinTrophyChallengeByIdAsync(int userId, int trophyId);

    Task<bool> JoinTrophyReadingTimeAsync(int userId, int trophyId, int minimumCriterionNumber);

    Task<bool> JoinTrophyReadingBooksAsync(int userId, int trophyId, int minimumCriterionNumber);

    Task<bool> JoinTrophyCategoryReaderAsync(int userId, int trophyId, string category, int minimumCriterionNumber);

    Task<bool> JoinTrophyActivitiesAsync(int userId, int trophyId, string minimumCriterionText);
    
    
    // -- LEAVE --
    Task<bool> LeaveTrophyChallengeByIdAsync(int userId, int trophyId);
    
    Task<bool> LeaveTrophyReadingTimeAsync(int userId, int trophyId);

    Task<bool> LeaveTrophyReadingBooksAsync(int userId, int trophyId);

    Task<bool> LeaveTrophyCategoryReaderAsync(int userId, int trophyId, string category);

    Task<bool> LeaveTrophyActivitiesAsync(int userId, int trophyId);
}