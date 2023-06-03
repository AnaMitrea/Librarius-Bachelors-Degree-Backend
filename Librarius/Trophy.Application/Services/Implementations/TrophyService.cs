using AutoMapper;
using Trophy.Application.Models;
using Trophy.DataAccess.Repositories;

namespace Trophy.Application.Services.Implementations;

public class TrophyService : ITrophyService
{
    private readonly IMapper _mapper;
    private readonly ITrophyRepository _trophyRepository;

    public TrophyService(IMapper mapper, ITrophyRepository trophyRepository)
    {
        _mapper = mapper;
        _trophyRepository = trophyRepository;
    }
    
    public async Task<bool> JoinTrophyChallengeByIdAsync(string username, int trophyId)
    {
        return await _trophyRepository.JoinTrophyChallengeByIdAsync(username, trophyId);
    }
    
    public async Task<bool> LeaveTrophyChallengeByIdAsync(string username, int trophyId)
    {
        return await _trophyRepository.LeaveTrophyChallengeByIdAsync(username, trophyId);
    }
    
    public async Task<IEnumerable<TrophyModel>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit)
    {
        // todo check if category is ok
        var trophies = await _trophyRepository.GetTrophiesByCategoryAsync(category, canTakeLimit);

        return _mapper.Map<IEnumerable<TrophyModel>>(trophies);
    }
    
    // all user completed trophies
    public async Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserAllCompletedTrophiesAsync(string username)
    {
        var trophies = await _trophyRepository.GetUserAllCompletedTrophiesAsync(username);
        
        return _mapper.Map<Dictionary<string, IEnumerable<TrophyModel>>>(trophies);
    }

    // all user completed trophies by category
    public async Task<IEnumerable<TrophyModel>> GetUserCompletedTrophiesByCategoryAsync(string username, string category)
    {
        var trophies = await _trophyRepository.GetUserCompletedTrophiesByCategoryAsync(username, category);
        
        return _mapper.Map<IEnumerable<TrophyModel>>(trophies);
    }

    // all user in progress trophies
    public async Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserInProgressTrophiesAsync(string username)
    {
        var trophies = await _trophyRepository.GetUserInProgressTrophiesAsync(username);
        
        return _mapper.Map<Dictionary<string, IEnumerable<TrophyModel>>>(trophies);
    }

    // all user in progress trophies by category
    public async Task<IEnumerable<TrophyModel>> GetUserInProgressTrophiesByCategoryAsync(string username, string category)
    {
        var trophies = await _trophyRepository.GetUserInProgressTrophiesByCategoryAsync(username, category);
        
        return _mapper.Map<IEnumerable<TrophyModel>>(trophies);
    }
}