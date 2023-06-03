namespace Trophy.DataAccess.Entities;

public class TrophyUserReward : Entity
{
    public int TrophyId { get; set; }
    
    public bool IsWon { get; set; }
    
    public Trophy Trophy { get; set; }
    
    public int UserId { get; set; }

    public TrophyUser User { get; set; }
}