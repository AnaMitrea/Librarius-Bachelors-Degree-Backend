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
    
    public async Task<bool> JoinTrophyChallengeByIdAsync(int userId, int trophyId)
    {
        return await _trophyRepository.JoinTrophyChallengeByIdAsync(userId, trophyId);
    }
    
    public async Task<bool> LeaveTrophyChallengeByIdAsync(int userId, int trophyId)
    {
        return await _trophyRepository.LeaveTrophyChallengeByIdAsync(userId, trophyId);
    }
    
    public async Task<IEnumerable<TrophyModel>> GetTrophiesByCategoryAsync(string category, bool canTakeLimit)
    {
        // todo check if category is ok
        var trophies = await _trophyRepository.GetTrophiesByCategoryAsync(category, canTakeLimit);

        return _mapper.Map<IEnumerable<TrophyModel>>(trophies);
    }
    
    // all user completed trophies
    public async Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserAllCompletedTrophiesAsync(int userId)
    {
        var trophies = await _trophyRepository.GetUserAllCompletedTrophiesAsync(userId);
        
        return _mapper.Map<Dictionary<string, IEnumerable<TrophyModel>>>(trophies);
    }

    // all user completed trophies by category
    public async Task<IEnumerable<TrophyModel>> GetUserCompletedTrophiesByCategoryAsync(int userId, string category)
    {
        var trophies = await _trophyRepository.GetUserCompletedTrophiesByCategoryAsync(userId, category);
        
        return _mapper.Map<IEnumerable<TrophyModel>>(trophies);
    }

    // all user in progress trophies
    public async Task<Dictionary<string, IEnumerable<TrophyModel>>> GetUserInProgressTrophiesAsync(int userId)
    {
        var trophies = await _trophyRepository.GetUserInProgressTrophiesAsync(userId);
        
        return _mapper.Map<Dictionary<string, IEnumerable<TrophyModel>>>(trophies);
    }

    // all user in progress trophies by category
    public async Task<IEnumerable<TrophyModel>> GetUserInProgressTrophiesByCategoryAsync(int userId, string category)
    {
        var trophies = await _trophyRepository.GetUserInProgressTrophiesByCategoryAsync(userId, category);
        
        return _mapper.Map<IEnumerable<TrophyModel>>(trophies);
    }
}