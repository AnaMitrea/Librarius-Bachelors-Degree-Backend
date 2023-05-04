namespace Identity.DataAccess.Entities;

public class TrophyAccount : Entity
{
    public int TrophyId { get; set; }
    
    public Trophy Trophy { get; set; }
    
    public int AccountId { get; set; }

    public Account Account { get; set; }
}