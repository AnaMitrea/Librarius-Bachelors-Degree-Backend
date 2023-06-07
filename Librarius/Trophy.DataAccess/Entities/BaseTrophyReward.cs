namespace Trophy.DataAccess.Entities;

public class BaseTrophyReward : Entity
{
    public int TrophyId { get; set; }
    
    public Trophy Trophy { get; set; }
    
    public int UserId { get; set; }
    
    public bool IsWon { get; set; }
    
    public int UserProgress { get; set; }
}