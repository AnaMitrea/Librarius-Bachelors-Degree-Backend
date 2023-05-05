using AutoMapper;
using Identity.Application.Models.Trophy;
using Identity.DataAccess.Repositories;

namespace Identity.Application.Services.Implementations;

public class TrophyService : ITrophyService
{
    private readonly IMapper _mapper;
    private readonly ITrophyRepository _trophyRepository;

    public TrophyService(IMapper mapper, ITrophyRepository trophyRepository)
    {
        _mapper = mapper;
        _trophyRepository = trophyRepository;
    }
    
    public async Task<IEnumerable<TrophyModel>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit)
    {
        // todo check if category is ok
        var trophies = await _trophyRepository.GetTrophiesByCategoryAsync(category, canTakeLimit);

        return _mapper.Map<IEnumerable<TrophyModel>>(trophies);
    }

    public async Task<IEnumerable<TrophyModel>> GetUserCompletedTrophiesByCategoryAsync(string username, string category)
    {
        var trophies = await _trophyRepository.GetUserCompletedTrophiesByCategoryAsync(username, category);
        
        return _mapper.Map<IEnumerable<TrophyModel>>(trophies);
    }

    public async Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserAllCompletedTrophiesAsync(string username)
    {
        var trophies = await _trophyRepository.GetUserAllCompletedTrophiesAsync(username);
        
        return _mapper.Map<Dictionary<string, IEnumerable<TrophyModel>>>(trophies);
    }
}