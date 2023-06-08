using Trophy.Application.Models.Trophy.Request.UserRewardActivity;
using Trophy.Application.Models.Trophy.Response;

namespace Trophy.Application.Services;

public interface ITrophyService
{
    Task<IEnumerable<TrophyModel>> CheckUserIfCanWinAsync(int userId);
    
    Task<bool> UpdateReadingTimeRewardActivityAsync(ReadingTimeUpdateActivityRequestModel requestModel, int userId);
    
    Task<bool> UpdateReadingBooksRewardActivityAsync(ReadingBooksUpdateActivityRequestModel requestModel, int userId);
    
    Task<bool> UpdateCategoryReaderRewardActivityAsync(CategoryReaderUpdateActivityRequestModel requestModel, int userId);
    
    // Task<bool> UpdateActivitiesRewardActivityAsync(ActivitiesUpdateActivityRequestModel requestModel, int userId);
    
    Task<IEnumerable<TrophyModel>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit);
    
    Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserAllCompletedTrophiesAsync(int userId);
    
    Task<IEnumerable<TrophyModel>> GetUserCompletedTrophiesByCategoryAsync(int userId, string category);
    
    Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserInProgressTrophiesAsync(int userId);
    
    Task<IEnumerable<TrophyModel>> GetUserInProgressTrophiesByCategoryAsync(int userId, string category);
    
    Task<bool> JoinTrophyChallengeByIdAsync(int userId, int trophyId);
    
    Task<bool> LeaveTrophyChallengeByIdAsync(int userId, int trophyId);
}