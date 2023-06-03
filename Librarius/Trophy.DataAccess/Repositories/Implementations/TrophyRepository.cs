﻿using Microsoft.EntityFrameworkCore;
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
    
    public async Task<bool> JoinTrophyChallengeByIdAsync(string username, int trophyId)
    {
        var account = await _dbContext.Users.SingleOrDefaultAsync(ac => ac.Username == username);
        if (account == null) throw new Exception("User not found.");

        var checkTrophy = await _dbContext.TrophyAccounts.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId & ta.UserId == account.Id);
        
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
            UserId = account.Id,
            IsWon = false
        };

        _dbContext.TrophyAccounts.Add(newTrophyChallenge);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LeaveTrophyChallengeByIdAsync(string username, int trophyId)
    {
        var account = await _dbContext.Users.SingleOrDefaultAsync(ac => ac.Username == username);
        if (account == null) throw new Exception("User not found.");

        var checkTrophy = await _dbContext.TrophyAccounts.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId & ta.UserId == account.Id);

        switch (checkTrophy)
        {
            case null:
                throw new Exception("Challenge not joined yet.");
            case {IsWon: true}:
                throw new Exception("Cannot leave a won challenge.");
        }

        _dbContext.TrophyAccounts.Remove(checkTrophy);

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
    public async Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserAllCompletedTrophiesAsync(string username)
    {
        var account = await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Username == username);
        if (account == default)
        {
            throw new Exception("User not found.");
        }

        await _dbContext.Entry(account)
            .Collection(user => user.Trophies)
            .Query()
            .Where(trophyAccount => trophyAccount.IsWon == true)
            .Include(trophyAccount => trophyAccount.Trophy)
            .LoadAsync();
        
        var trophiesByCategory = account.Trophies
            .Select(trophyAccount => trophyAccount.Trophy)
            .GroupBy(trophy => trophy.Category)
            .ToDictionary(group => group.Key,
                group => group.AsEnumerable());

        return trophiesByCategory;
    }
    
    // all user completed trophies by category
    public async Task<IEnumerable<Entities.Trophy>> GetUserCompletedTrophiesByCategoryAsync(string username, string category)
    {
        var account = await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Username == username);
        if (account == default)
        {
            throw new Exception("User not found.");
        }

        await _dbContext.Entry(account)
            .Collection(user => user.Trophies)
            .Query()
            .Where(trophyAccount => trophyAccount.IsWon == true)
            .Include(trophyAccount => trophyAccount.Trophy)
            .LoadAsync();
        
        var completedTrophies = account.Trophies
            .Where(trophyAccount => trophyAccount.Trophy.Category == category)
            .Select(trophyAccount => trophyAccount.Trophy);

        return completedTrophies;
    }
    
    // all user in progress trophies
    public async Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserInProgressTrophiesAsync(string username)
    {
        var account = await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Username == username);
        if (account == default)
        {
            throw new Exception("User not found.");
        }

        await _dbContext.Entry(account)
            .Collection(user => user.Trophies)
            .Query()
            .Where(trophyAccount => trophyAccount.IsWon == false)
            .Include(trophyAccount => trophyAccount.Trophy)
            .LoadAsync();
        
        var trophiesByCategory = account.Trophies
            .Select(trophyAccount => trophyAccount.Trophy)
            .GroupBy(trophy => trophy.Category)
            .ToDictionary(group => group.Key,
                group => group.AsEnumerable());

        return trophiesByCategory;
    }

    // all user in progress trophies by category
    public async Task<IEnumerable<Entities.Trophy>> GetUserInProgressTrophiesByCategoryAsync(string username, string category)
    {
        var account = await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Username == username);
        if (account == default)
        {
            throw new Exception("User not found.");
        }

        await _dbContext.Entry(account)
            .Collection(user => user.Trophies)
            .Query()
            .Where(trophyAccount => trophyAccount.IsWon == false)
            .Include(trophyAccount => trophyAccount.Trophy)
            .LoadAsync();
        
        var completedTrophies = account.Trophies
            .Where(trophyAccount => trophyAccount.Trophy.Category == category)
            .Select(trophyAccount => trophyAccount.Trophy);

        return completedTrophies;
    }
}