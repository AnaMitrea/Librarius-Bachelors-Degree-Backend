namespace Trophy.DataAccess.Entities;

public class Trophy : Entity
{
    public string Title { get; set; }
    
    public string Category { get; set; }
    
    public string Instructions { get; set; }
    
    public string ImageSrcPath { get; set; }
    
    // many-to-many: trophies -> accounts
    public IEnumerable<TrophyUserReward> TrophyAccounts { get; set; }
}