namespace Trophy.DataAccess.Entities;

public class TrophyRewardCategoryReader : BaseTrophyReward
{
    public string Category { get; set; }
    
    public int MinimumCriterionNumber { get; set; }
}