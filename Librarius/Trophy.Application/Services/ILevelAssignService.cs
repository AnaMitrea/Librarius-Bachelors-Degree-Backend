using Trophy.Application.Models.LevelAssign.Request;
using Trophy.Application.Models.LevelAssign.Response;

namespace Trophy.Application.Services;

public interface ILevelAssignService
{
    Task AddUserPointsByUserIdAsync(int userId, int points);
    
    Task<IEnumerable<LevelModel>> GetLevels(bool orderedAsc);
    Task<string> GetLevelByPointsAsync(LevelRequestModel requestModel);
    Task<string> GetNextLevelByPointsAsync(LevelRequestModel requestModel);
}