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
    
    public async Task<IEnumerable<Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeMax4 = false)
    {
        IEnumerable<Trophy> trophies;
        if (canTakeMax4)
        {
            trophies = await _databaseContext.Trophies
                        .Where(trophy => trophy.Category == category)
                        .OrderBy(trophy => trophy.Id)
                        .Take(4)
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

    public async Task<IEnumerable<Trophy>> GetUserCompletedTrophiesByCategoryAsync(string username, string category)
    {
        var account = await _databaseContext.Accounts
            .SingleOrDefaultAsync(user => user.Username == username);
        if (account == default)
        {
            throw new Exception("User not found.");
        }

        await _databaseContext.Entry(account)
            .Collection(user => user.TrophyAccounts)
            .Query()
            .Include(trophyAccount => trophyAccount.Trophy)
            .LoadAsync();
        
        var completedTrophies = account.TrophyAccounts
            .Where(trophyAccount => trophyAccount.Trophy.Category == category)
            .Select(trophyAccount => trophyAccount.Trophy);

        return completedTrophies;
    }
}