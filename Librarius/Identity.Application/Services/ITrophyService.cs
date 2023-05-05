using Identity.Application.Models.Trophy;

namespace Identity.Application.Services;

public interface ITrophyService
{
    Task<IEnumerable<TrophyModel>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit);
    
    Task<IEnumerable<TrophyModel>> GetUserCompletedTrophiesByCategoryAsync(string username, string category);
}