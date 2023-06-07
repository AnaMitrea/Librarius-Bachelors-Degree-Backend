using Trophy.DataAccess.Entities;

namespace Trophy.DataAccess.Repositories;

public interface ILevelAssignRepository
{
    Task<IEnumerable<Level>> GetLevels();
    
    Task<string> GetLevelByPointsAsync(int points);
    Task<string> GetNextLevelByPointsAsync(int points);
}