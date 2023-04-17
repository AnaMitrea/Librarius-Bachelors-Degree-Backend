namespace Identity.DataAccess.Entities;

public class Account : Entity
{
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string Role { get; set; }
}