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
    
    public async Task<IEnumerable<TrophyModel>> GetTrophiesByCategoryAsync(string category, bool canTakeMax16)
    {
        // todo check if category is ok
        var trophies = await _trophyRepository.GetTrophiesByCategoryAsync(category, canTakeMax16);

        return _mapper.Map<IEnumerable<TrophyModel>>(trophies);
    }
}