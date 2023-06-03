namespace Trophy.DataAccess.Entities;

public class TrophyUser : Entity
{
    public string Username { get; set; }
    
    // many-to-many: User -> Trophies
    public IEnumerable<TrophyUserReward> Trophies { get; set; }
}