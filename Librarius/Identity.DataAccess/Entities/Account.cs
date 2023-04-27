namespace Identity.DataAccess.Entities;

public class Account : Entity
{
    public string Username { get; set; }

    public string Password { get; set; }
    
    public string Role { get; set; }
    
    public string Email { get; set; }
    
    public string Level { get; set; }
    
    public int Points { get; set; }
    
    public int? LongestStreak { get; set; }
    
    public DateTime? LastLogin { get; set; } 
}