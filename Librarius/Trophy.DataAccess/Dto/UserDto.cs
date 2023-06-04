namespace Trophy.DataAccess.Dto;

public class UserDto
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }

    public string Role { get; set; }
    
    public string Level { get; set; }
    
    public int Points { get; set; }
    
    public int LongestStreak { get; set; }
    
    public int CurrentStreak { get; set; }
    
    public string LastLogin { get; set; } 
}