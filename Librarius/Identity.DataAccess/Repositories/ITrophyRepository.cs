using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Repositories;

public interface ITrophyRepository
{
    Task<IEnumerable<Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit = false);

    Task<IEnumerable<Trophy>> GetUserCompletedTrophiesByCategoryAsync(string username, string category);
    
    Task<Dictionary<string, IEnumerable<Trophy>>> GetUserAllCompletedTrophiesAsync(string username);
}