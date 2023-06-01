using Identity.DataAccess.Entities;
using Identity.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Repositories.Implementations;

public class TrophyRepository : ITrophyRepository
{
    private readonly DatabaseContext _dbContext;

    public TrophyRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeMax4 = false)
    {
        IEnumerable<Trophy> trophies;
        if (canTakeMax4)
        {
            trophies = await _dbContext.Trophies
                        .Where(trophy => trophy.Category == category)
                        .OrderBy(trophy => trophy.Id)
                        .Take(4)
                        .ToListAsync();
        }
        else
        {
            trophies = await _dbContext.Trophies
                .Where(trophy => trophy.Category == category)
                .OrderBy(trophy => trophy.Id)
                .ToListAsync();
        }
        
        return trophies;
    }

    // all user completed trophies
    public async Task<Dictionary<string, IEnumerable<Trophy>>> GetUserAllCompletedTrophiesAsync(string username)
    {
        var account = await _dbContext.Accounts
            .SingleOrDefaultAsync(user => user.Username == username);
        if (account == default)
        {
            throw new Exception("User not found.");
        }

        await _dbContext.Entry(account)
            .Collection(user => user.TrophyAccounts)
            .Query()
            .Where(trophyAccount => trophyAccount.IsWon == true)
            .Include(trophyAccount => trophyAccount.Trophy)
            .LoadAsync();
        
        var trophiesByCategory = account.TrophyAccounts
            .Select(trophyAccount => trophyAccount.Trophy)
            .GroupBy(trophy => trophy.Category)
            .ToDictionary(group => group.Key,
                group => group.AsEnumerable());

        return trophiesByCategory;
    }
    
    // all user completed trophies by category
    public async Task<IEnumerable<Trophy>> GetUserCompletedTrophiesByCategoryAsync(string username, string category)
    {
        var account = await _dbContext.Accounts
            .SingleOrDefaultAsync(user => user.Username == username);
        if (account == default)
        {
            throw new Exception("User not found.");
        }

        await _dbContext.Entry(account)
            .Collection(user => user.TrophyAccounts)
            .Query()
            .Where(trophyAccount => trophyAccount.IsWon == true)
            .Include(trophyAccount => trophyAccount.Trophy)
            .LoadAsync();
        
        var completedTrophies = account.TrophyAccounts
            .Where(trophyAccount => trophyAccount.Trophy.Category == category)
            .Select(trophyAccount => trophyAccount.Trophy);

        return completedTrophies;
    }
    
    // all user in progress trophies
    public Task<Dictionary<string, IEnumerable<Trophy>>> GetUserInProgressTrophiesAsync(string username)
    {
        throw new NotImplementedException();
    }

    // all user in progress trophies by category
    public Task<IEnumerable<Trophy>> GetUserInProgressTrophiesByCategoryAsync(string username, string category)
    {
        throw new NotImplementedException();
    }
}