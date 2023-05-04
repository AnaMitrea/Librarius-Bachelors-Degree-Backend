using Identity.DataAccess.Entities;
using Identity.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Repositories.Implementations;

public class TrophyRepository : ITrophyRepository
{
    private readonly DatabaseContext _databaseContext;

    public TrophyRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task<IEnumerable<Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeMax16 = false)
    {
        IEnumerable<Trophy> trophies;
        if (canTakeMax16)
        {
            trophies = await _databaseContext.Trophies
                        .Where(trophy => trophy.Category == category)
                        .OrderBy(trophy => trophy.Id)
                        .Take(16)
                        .ToListAsync();
        }
        else
        {
            trophies = await _databaseContext.Trophies
                .Where(trophy => trophy.Category == category)
                .OrderBy(trophy => trophy.Id)
                .ToListAsync();
        }
        
        return trophies;
    }
}