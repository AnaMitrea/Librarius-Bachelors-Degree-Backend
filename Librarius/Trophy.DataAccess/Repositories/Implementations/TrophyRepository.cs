using Microsoft.EntityFrameworkCore;
using Trophy.DataAccess.Entities;
using Trophy.DataAccess.Persistence;

namespace Trophy.DataAccess.Repositories.Implementations;

public class TrophyRepository : ITrophyRepository
{
    private readonly DatabaseContext _dbContext;

    public TrophyRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> JoinTrophyChallengeByIdAsync(int userId, int trophyId)
    {
        var checkTrophy = await _dbContext.TrophyUserReward.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId & ta.UserId == userId);
        
        switch (checkTrophy)
        {
            case { IsWon: true }:
                throw new Exception("Challenge already won.");
            case { IsWon: false }:
                throw new Exception("Challenge already joined.");
        }
        

        var newTrophyChallenge = new TrophyUserReward
        {
            TrophyId = trophyId,
            UserId = userId,
            IsWon = false
        };

        _dbContext.TrophyUserReward.Add(newTrophyChallenge);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LeaveTrophyChallengeByIdAsync(int userId, int trophyId)
    {
        var checkTrophy = await _dbContext.TrophyUserReward.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId & ta.UserId == userId);

        switch (checkTrophy)
        {
            case null:
                throw new Exception("Challenge not joined yet.");
            case {IsWon: true}:
                throw new Exception("Cannot leave a won challenge.");
        }

        _dbContext.TrophyUserReward.Remove(checkTrophy);

        await _dbContext.SaveChangesAsync();

        return false;
    }
    
    public async Task<IEnumerable<Entities.Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeMax4 = false)
    {
        IEnumerable<Entities.Trophy> trophies;
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
    public async Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserAllCompletedTrophiesAsync(int userId)
    {
        var completedTrophies = await _dbContext.TrophyUserReward
            .Where(trophyUserReward => trophyUserReward.UserId == userId && trophyUserReward.IsWon)
            .Select(trophyUserReward => trophyUserReward.Trophy)
            .ToListAsync();

        var trophiesByCategory = completedTrophies
            .GroupBy(trophy => trophy.Category)
            .ToDictionary(group => group.Key, group => group.AsEnumerable());

        return trophiesByCategory;
    }
    
    // all user completed trophies by category
    public async Task<IEnumerable<Entities.Trophy>> GetUserCompletedTrophiesByCategoryAsync(int userId, string category)
    {
        var completedTrophies = await _dbContext.TrophyUserReward
            .Where(trophyUserReward => trophyUserReward.UserId == userId && trophyUserReward.IsWon)
            .Select(trophyUserReward => trophyUserReward.Trophy)
            .Where(trophy => trophy.Category == category)
            .ToListAsync();
        
        return completedTrophies;
    }
    
    // all user in progress trophies
    public async Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserInProgressTrophiesAsync(int userId)
    {
        var inProgressTrophies = await _dbContext.TrophyUserReward
            .Where(trophyUserReward => trophyUserReward.UserId == userId && !trophyUserReward.IsWon)
            .Select(trophyUserReward => trophyUserReward.Trophy)
            .ToListAsync();

        var trophiesByCategory = inProgressTrophies
            .GroupBy(trophy => trophy.Category)
            .ToDictionary(group => group.Key, group => group.AsEnumerable());

        return trophiesByCategory;
    }

    // all user in progress trophies by category
    public async Task<IEnumerable<Entities.Trophy>> GetUserInProgressTrophiesByCategoryAsync(int userId, string category)
    {
        var inProgressTrophies = await _dbContext.TrophyUserReward
            .Where(trophyUserReward => trophyUserReward.UserId == userId && !trophyUserReward.IsWon)
            .Select(trophyUserReward => trophyUserReward.Trophy)
            .Where(trophy => trophy.Category == category)
            .ToListAsync();

        return inProgressTrophies;
    }
}