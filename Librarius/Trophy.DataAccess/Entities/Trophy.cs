namespace Trophy.DataAccess.Entities;

public class Trophy : Entity
{
    public string Title { get; set; }
    
    public string Category { get; set; }
    
    public string Instructions { get; set; }
    
    public string ImageSrcPath { get; set; }
    
    public int? MinimumCriterionNumber { get; set; }
    
    public string? MinimumCriterionText { get; set; }

    // many-to-many: trophies -> accounts
    public IEnumerable<TrophyUserReward> TrophyAccounts { get; set; }
    
    public IEnumerable<TrophyRewardReadingBooks> TrophiesRewardReadingBooks { get; set; }
    
    public IEnumerable<TrophyRewardReadingTime> TrophiesRewardReadingTime { get; set; }
    
    public IEnumerable<TrophyRewardCategoryReader> TrophiesRewardCategoryReaders { get; set; }
    
    public IEnumerable<TrophyRewardActivities> TrophiesRewardActivities { get; set; }
}