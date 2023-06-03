using Trophy.Application.Models;

namespace Trophy.Application.Services;

public interface ITrophyService
{
    Task<IEnumerable<TrophyModel>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit);
    
    Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserAllCompletedTrophiesAsync(string username);
    
    Task<IEnumerable<TrophyModel>> GetUserCompletedTrophiesByCategoryAsync(string username, string category);
    
    Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserInProgressTrophiesAsync(string username);
    
    Task<IEnumerable<TrophyModel>> GetUserInProgressTrophiesByCategoryAsync(string username, string category);
    
    Task<bool> JoinTrophyChallengeByIdAsync(string username, int trophyId);
    
    Task<bool> LeaveTrophyChallengeByIdAsync(string username, int trophyId);
}