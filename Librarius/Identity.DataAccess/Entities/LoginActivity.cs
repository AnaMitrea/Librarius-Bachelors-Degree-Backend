namespace Identity.DataAccess.Entities;

public class LoginActivity : Entity
{
    public string DateTimestamp { get; set; }
    
    public int AccountId { get; set; }
    
    public Account Account { get; set; }
}