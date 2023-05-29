namespace Identity.DataAccess.DTOs;

public class UserLeaderboardByPointsDto
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string NameInitial 
        => string.IsNullOrEmpty(Username) 
            ? string.Empty 
            : Username[..1].ToUpper();
    
    public int Position { get; set; }
    
    public int Points { get; set; }
}