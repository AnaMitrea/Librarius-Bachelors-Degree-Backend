using Trophy.Application.Models;

namespace Trophy.Application.Services;

public interface ITrophyService
{
    Task<bool> CheckUserIfCanWinAsync(int userId);
    
    Task<IEnumerable<TrophyModel>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit);
    
    Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserAllCompletedTrophiesAsync(int userId);
    
    Task<IEnumerable<TrophyModel>> GetUserCompletedTrophiesByCategoryAsync(int userId, string category);
    
    Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserInProgressTrophiesAsync(int userId);
    
    Task<IEnumerable<TrophyModel>> GetUserInProgressTrophiesByCategoryAsync(int userId, string category);
    
    Task<bool> JoinTrophyChallengeByIdAsync(int userId, int trophyId);
    
    Task<bool> LeaveTrophyChallengeByIdAsync(int userId, int trophyId);
}