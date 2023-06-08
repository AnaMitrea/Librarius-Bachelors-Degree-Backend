namespace Trophy.DataAccess.Entities;

public class TrophyRewardCategoryReader : BaseTrophyReward
{
    public int CategoryId { get; set; }
    
    public int MinimumCriterionNumber { get; set; }
}