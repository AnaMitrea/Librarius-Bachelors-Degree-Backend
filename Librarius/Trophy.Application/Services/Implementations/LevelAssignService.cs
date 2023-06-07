using AutoMapper;
using Trophy.Application.Models.LevelAssign.Request;
using Trophy.Application.Models.LevelAssign.Response;
using Trophy.DataAccess.Repositories;

namespace Trophy.Application.Services.Implementations;

public class LevelAssignService : ILevelAssignService
{
    private readonly IMapper _mapper;
    private readonly ILevelAssignRepository _levelRepository;

    public LevelAssignService(IMapper mapper, ILevelAssignRepository levelRepository)
    {
        _mapper = mapper;
        _levelRepository = levelRepository;
    }

    public async Task<string> GetLevelByPointsAsync(LevelRequestModel requestModel)
    {
        if (requestModel.Points < 0) throw new Exception("Points cannot be negative.");
        
        return await _levelRepository.GetLevelByPointsAsync(requestModel.Points);
    }

    public async Task<string> GetNextLevelByPointsAsync(LevelRequestModel requestModel)
    {
        if (requestModel.Points < 0) throw new Exception("Points cannot be negative.");
        
        return await _levelRepository.GetNextLevelByPointsAsync(requestModel.Points);
    }

    public async Task<IEnumerable<LevelModel>> GetLevels(bool orderedAsc)
    {
        var response = await _levelRepository.GetLevels();

        response = orderedAsc ? response.OrderBy(l => l.MinPoints) : response.OrderByDescending(l => l.MinPoints);
            
        return _mapper.Map<IEnumerable<LevelModel>>(response);
    }
}