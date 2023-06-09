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

    public async Task<IEnumerable<string>> GetAllTrophyCategoriesAsync()
    {
        return await _dbContext.Trophies
            .Select(tr => tr.Category)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<Entities.Trophy>> CheckUserIfCanWinAsync(int userId)
    {
        var inProgressReadingTimeChallenges = await GetUserInProgressChallengesForReadingTimeAsync(userId);
        var inProgressReadingBooksChallenges = await GetUserInProgressChallengesForReadingBooksAsync(userId);
        var inProgressCategoryReaderChallenges = await GetUserInProgressChallengesForCategoryReaderAsync(userId);
        var inProgressActivitiesChallenges = await GetUserInProgressChallengesForActivitiesAsync(userId);

        var trophies = new List<Entities.Trophy>();

        // Reading Time Challenges
        foreach (var challenge in inProgressReadingTimeChallenges)
        {
            if (challenge.UserProgress < challenge.MinimumCriterionNumber) continue;
            
            challenge.IsWon = true;
            trophies.Add(challenge.Trophy);
        }

        // Reading Books Challenges
        foreach (var challenge in inProgressReadingBooksChallenges)
        {
            if (challenge.UserProgress < challenge.MinimumCriterionNumber) continue;

            challenge.IsWon = true;
            trophies.Add(challenge.Trophy);
        }

        // Category Reader Challenges
        foreach (var challenge in inProgressCategoryReaderChallenges)
        {
            if (challenge.UserProgress < challenge.MinimumCriterionNumber) continue;
            
            challenge.IsWon = true;
            trophies.Add(challenge.Trophy);
        }

        // Activities Challenges
        // foreach (var challenge in inProgressActivitiesChallenges)
        // {
        //     // ----TODO FIND A WAY OF IMPLEMENTATION----
        //     
        //     
        //     if (challenge.UserProgress < challenge.MinimumCriterionNumber) continue;
        //     
        //     challenge.IsWon = true;
        //     trophies.Add(challenge.Trophy);
        // }

        if (trophies.Any())
        {
            await _dbContext.SaveChangesAsync();
        }

        return trophies;
    }

    public async Task<bool> UpdateReadingTimeRewardActivityAsync(int userId, int readingHoursCounter)
    {
        var inProgressTrophies = await GetUserInProgressChallengesForReadingTimeAsync(userId);

        foreach (var trophy in inProgressTrophies)
        {
            trophy.UserProgress = readingHoursCounter;
        }
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateReadingBooksRewardActivityAsync(int userId, int readingBooksCounter)
    {
        var inProgressTrophies = await GetUserInProgressChallengesForReadingBooksAsync(userId);

        foreach (var trophy in inProgressTrophies)
        {
            trophy.UserProgress = readingBooksCounter;
        }
        
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateCategoryReaderRewardActivityAsync(int userId, int readingBooksCounter)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> JoinTrophyChallengeByIdAsync(int userId, int trophyId)
    {
        var trophy = await _dbContext.Trophies.FindAsync(trophyId);

        if (trophy == null) throw new Exception("Invalid trophy id.");

        switch (trophy.Category)
        {
            case "Reading Time":
                if (trophy.MinimumCriterionNumber != null)
                {
                    await JoinTrophyReadingTimeAsync(
                        userId,
                        trophyId,
                        (int)trophy.MinimumCriterionNumber
                    );
                }
                
                break;
            case "Reading Books":
                if (trophy.MinimumCriterionNumber != null)
                {
                    await JoinTrophyReadingBooksAsync(
                        userId,
                        trophyId,
                        (int)trophy.MinimumCriterionNumber
                    );
                }

                break;
            case "Category Reader":
                if (trophy is { MinimumCriterionNumber: not null, BookCategoryId: not null })
                {
                    await JoinTrophyCategoryReaderAsync(
                        userId,
                        trophyId,
                        (int)trophy.BookCategoryId,
                        (int)trophy.MinimumCriterionNumber
                    );
                }

                break;
            case "Activities":
                if (trophy.MinimumCriterionText != null)
                {
                    await JoinTrophyActivitiesAsync(
                        userId,
                        trophyId,
                        trophy.MinimumCriterionText
                    );
                }

                break;
        };

        return true;
    }

    public async Task<bool> JoinTrophyReadingTimeAsync(int userId, int trophyId, int minimumCriterionNumber)
    {
        var checkTrophy = await _dbContext.TrophyRewardReadingTime.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId && ta.UserId == userId);
        
        switch (checkTrophy)
        {
            case { IsWon: true }:
                throw new Exception("Challenge already won.");
            case { IsWon: false }:
                throw new Exception("Challenge already joined.");
        }

        var newTrophyChallenge = new TrophyRewardReadingTime
        {
            TrophyId = trophyId,
            UserId = userId,
            IsWon = false,
            UserProgress = 0,
            MinimumCriterionNumber = minimumCriterionNumber
        };

        _dbContext.TrophyRewardReadingTime.Add(newTrophyChallenge);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> JoinTrophyReadingBooksAsync(int userId, int trophyId, int minimumCriterionNumber)
    {
        var checkTrophy = await _dbContext.TrophyRewardReadingBooks.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId && ta.UserId == userId);
        
        switch (checkTrophy)
        {
            case { IsWon: true }:
                throw new Exception("Challenge already won.");
            case { IsWon: false }:
                throw new Exception("Challenge already joined.");
        }

        var newTrophyChallenge = new TrophyRewardReadingBooks
        {
            TrophyId = trophyId,
            UserId = userId,
            IsWon = false,
            UserProgress = 0,
            MinimumCriterionNumber = minimumCriterionNumber
        };

        _dbContext.TrophyRewardReadingBooks.Add(newTrophyChallenge);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> JoinTrophyCategoryReaderAsync(
        int userId, int trophyId, int categoryId, int minimumCriterionNumber)
    {
        var checkTrophy = await _dbContext.TrophyRewardCategoryReader.SingleOrDefaultAsync(ta => 
            ta.TrophyId == trophyId && ta.UserId == userId);
        
        switch (checkTrophy)
        {
            case { IsWon: true }:
                throw new Exception("Challenge already won.");
            case { IsWon: false }:
                throw new Exception("Challenge already joined.");
        }

        var newTrophyChallenge = new TrophyRewardCategoryReader
        {
            TrophyId = trophyId,
            UserId = userId,
            CategoryId = categoryId,
            IsWon = false,
            UserProgress = 0,
            MinimumCriterionNumber = minimumCriterionNumber
        };

        _dbContext.TrophyRewardCategoryReader.Add(newTrophyChallenge);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> JoinTrophyActivitiesAsync(int userId, int trophyId, string minimumCriterionText)
    {
        var checkTrophy = await _dbContext.TrophyRewardActivities.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId && ta.UserId == userId);
        
        switch (checkTrophy)
        {
            case { IsWon: true }:
                throw new Exception("Challenge already won.");
            case { IsWon: false }:
                throw new Exception("Challenge already joined.");
        }

        var newTrophyChallenge = new TrophyRewardActivities
        {
            TrophyId = trophyId,
            UserId = userId,
            IsWon = false,
            UserProgress = 0,
            MinimumCriterionText = minimumCriterionText
        };

        _dbContext.TrophyRewardActivities.Add(newTrophyChallenge);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LeaveTrophyChallengeByIdAsync(int userId, int trophyId)
    {
        var trophy = await _dbContext.Trophies.FindAsync(trophyId);

        if (trophy == null) throw new Exception("Invalid trophy id.");

        switch (trophy.Category)
        {
            case "Reading Time":
                if (trophy.MinimumCriterionNumber != null)
                {
                    await LeaveTrophyReadingTimeAsync(userId, trophyId);
                }
                
                break;
            case "Reading Books":
                if (trophy.MinimumCriterionNumber != null)
                {
                    await LeaveTrophyReadingBooksAsync(userId, trophyId);
                }

                break;
            case "Category Reader":
                if (trophy.MinimumCriterionNumber != null)
                {
                    await LeaveTrophyCategoryReaderAsync(userId, trophyId, trophy.Category);
                }

                break;
            case "Activities":
                if (trophy.MinimumCriterionText != null)
                {
                    await LeaveTrophyActivitiesAsync(userId, trophyId);
                }

                break;
        };

        return true;
    }

    public async Task<bool> LeaveTrophyReadingTimeAsync(int userId, int trophyId)
    {
        var checkTrophy = await _dbContext.TrophyRewardReadingTime.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId & ta.UserId == userId);

        switch (checkTrophy)
        {
            case null:
                throw new Exception("Challenge not joined yet.");
            case {IsWon: true}:
                throw new Exception("Cannot leave a won challenge.");
        }

        _dbContext.TrophyRewardReadingTime.Remove(checkTrophy);

        await _dbContext.SaveChangesAsync();

        return false;
    }

    public async Task<bool> LeaveTrophyReadingBooksAsync(int userId, int trophyId)
    {
        var checkTrophy = await _dbContext.TrophyRewardReadingBooks.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId & ta.UserId == userId);

        switch (checkTrophy)
        {
            case null:
                throw new Exception("Challenge not joined yet.");
            case {IsWon: true}:
                throw new Exception("Cannot leave a won challenge.");
        }

        _dbContext.TrophyRewardReadingBooks.Remove(checkTrophy);

        await _dbContext.SaveChangesAsync();

        return false;
    }

    public async Task<bool> LeaveTrophyCategoryReaderAsync(int userId, int trophyId, string category)
    {
        var checkTrophy = await _dbContext.TrophyRewardCategoryReader.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId & ta.UserId == userId);

        switch (checkTrophy)
        {
            case null:
                throw new Exception("Challenge not joined yet.");
            case {IsWon: true}:
                throw new Exception("Cannot leave a won challenge.");
        }

        _dbContext.TrophyRewardCategoryReader.Remove(checkTrophy);

        await _dbContext.SaveChangesAsync();

        return false;
    }

    public async Task<bool> LeaveTrophyActivitiesAsync(int userId, int trophyId)
    {
        var checkTrophy = await _dbContext.TrophyRewardActivities.SingleOrDefaultAsync(
            ta => ta.TrophyId == trophyId & ta.UserId == userId);

        switch (checkTrophy)
        {
            case null:
                throw new Exception("Challenge not joined yet.");
            case {IsWon: true}:
                throw new Exception("Cannot leave a won challenge.");
        }

        _dbContext.TrophyRewardActivities.Remove(checkTrophy);

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
        var trophiesByCategory = new Dictionary<string, IEnumerable<Entities.Trophy>>();

        var categories = await GetAllTrophyCategoriesAsync();

        foreach (var category in categories)
        {
            var completedTrophies = await GetUserCompletedTrophiesByCategoryAsync(userId, category);
            trophiesByCategory.Add(category, completedTrophies);
        }

        return trophiesByCategory;
    }
    
    // all user completed trophies by category
    public async Task<IEnumerable<Entities.Trophy>> GetUserCompletedTrophiesByCategoryAsync(int userId, string category)
    {
        return category switch
        {
            "Reading Time" => await _dbContext.TrophyRewardReadingTime
                .Where(trophy => trophy.UserId == userId && trophy.IsWon)
                .Select(trophy => trophy.Trophy)
                .ToListAsync(),
            "Reading Books" => await _dbContext.TrophyRewardReadingBooks
                .Where(trophy => trophy.UserId == userId && trophy.IsWon)
                .Select(trophy => trophy.Trophy)
                .ToListAsync(),
            "Category Reader" => await _dbContext.TrophyRewardCategoryReader
                .Where(trophy => trophy.UserId == userId && trophy.IsWon)
                .Select(trophy => trophy.Trophy)
                .ToListAsync(),
            "Activities" => await _dbContext.TrophyRewardActivities
                .Where(trophy => trophy.UserId == userId && trophy.IsWon)
                .Select(trophy => trophy.Trophy)
                .ToListAsync(),
            _ => Enumerable.Empty<Entities.Trophy>()
        };
    }
    
    // all user in progress trophies
    public async Task<Dictionary<string, IEnumerable<Entities.Trophy>>> GetUserInProgressTrophiesAsync(int userId)
    {
        var trophiesByCategory = new Dictionary<string, IEnumerable<Entities.Trophy>>();

        var categories = await GetAllTrophyCategoriesAsync();

        foreach (var category in categories)
        {
            var inProgressTrophies = await GetUserInProgressTrophiesByCategoryAsync(userId, category);
            trophiesByCategory.Add(category, inProgressTrophies);
        }

        return trophiesByCategory;
    }

    // all user in progress trophies by category
    public async Task<IEnumerable<Entities.Trophy>> GetUserInProgressTrophiesByCategoryAsync(int userId, string category)
    {
        return category switch
        {
            "Reading Time" => await _dbContext.TrophyRewardReadingTime
                .Where(trophyUserReward => trophyUserReward.UserId == userId && !trophyUserReward.IsWon)
                .Select(trophyUserReward => trophyUserReward.Trophy)
                .Where(trophy => trophy.Category == category)
                .ToListAsync(),
            "Reading Books" => await _dbContext.TrophyRewardReadingBooks
                .Where(trophyUserReward => trophyUserReward.UserId == userId && !trophyUserReward.IsWon)
                .Select(trophyUserReward => trophyUserReward.Trophy)
                .Where(trophy => trophy.Category == category)
                .ToListAsync(),
            "Category Reader" => await _dbContext.TrophyRewardCategoryReader
                .Where(trophyUserReward => trophyUserReward.UserId == userId && !trophyUserReward.IsWon)
                .Select(trophyUserReward => trophyUserReward.Trophy)
                .Where(trophy => trophy.Category == category)
                .ToListAsync(),
            "Activities" => await _dbContext.TrophyRewardActivities
                .Where(trophyUserReward => trophyUserReward.UserId == userId && !trophyUserReward.IsWon)
                .Select(trophyUserReward => trophyUserReward.Trophy)
                .Where(trophy => trophy.Category == category)
                .ToListAsync(),
            _ => Enumerable.Empty<Entities.Trophy>()
        };
    }
    
    // all user in progress challenges READING TIME
    public async Task<IEnumerable<TrophyRewardReadingTime>> GetUserInProgressChallengesForReadingTimeAsync(int userId)
    {
        var inProgressChallenges = await _dbContext.TrophyRewardReadingTime
            .Where(challenge => challenge.UserId == userId && !challenge.IsWon)
            .Include(c => c.Trophy)
            .ToListAsync();

        return inProgressChallenges;
    }
    
    // all user in progress challenges READING BOOKS
    public async Task<IEnumerable<TrophyRewardReadingBooks>> GetUserInProgressChallengesForReadingBooksAsync(int userId)
    {
        var inProgressChallenges = await _dbContext.TrophyRewardReadingBooks
            .Where(challenge => challenge.UserId == userId && !challenge.IsWon)
            .Include(c => c.Trophy)
            .ToListAsync();

        return inProgressChallenges;
    }
    
    // all user in progress challenges CATEGORY READER
    public async Task<IEnumerable<TrophyRewardCategoryReader>> GetUserInProgressChallengesForCategoryReaderAsync(int userId)
    {
        var inProgressChallenges = await _dbContext.TrophyRewardCategoryReader
            .Where(challenge => challenge.UserId == userId && !challenge.IsWon)
            .Include(c => c.Trophy)
            .ToListAsync();

        return inProgressChallenges;
    }
    
    // all user in progress challenges ACTIVITIES
    public async Task<IEnumerable<TrophyRewardActivities>> GetUserInProgressChallengesForActivitiesAsync(int userId)
    {
        var inProgressChallenges = await _dbContext.TrophyRewardActivities
            .Where(challenge => challenge.UserId == userId && !challenge.IsWon)
            .Include(c => c.Trophy)
            .ToListAsync();

        return inProgressChallenges;
    }
}