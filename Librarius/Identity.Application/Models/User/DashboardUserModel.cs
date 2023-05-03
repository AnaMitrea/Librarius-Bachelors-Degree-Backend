namespace Identity.Application.Models.User;

public class DashboardUserModel
{
    public string Username { get; set; }
    
    public string Level { get; set; }
    
    public int Points { get; set; }
    
    public int LongestStreak { get; set; }
    
    public int CurrentStreak { get; set; }
}