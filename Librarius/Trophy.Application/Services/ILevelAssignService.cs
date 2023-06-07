using Trophy.Application.Models.LevelAssign.Request;
using Trophy.Application.Models.LevelAssign.Response;

namespace Trophy.Application.Services;

public interface ILevelAssignService
{
    Task<IEnumerable<LevelModel>> GetLevels(bool orderedAsc);
    
    Task<string> GetLevelByPointsAsync(LevelRequestModel requestModel);
    Task<string> GetNextLevelByPointsAsync(LevelRequestModel requestModel);
}