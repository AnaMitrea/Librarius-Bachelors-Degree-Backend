namespace Library.Application.Models.LibraryUser.Response;

public class UserLeaderboardByMinutes : UserResponseModel
{
    public int Position { get; set; }
    
    public int MinutesLogged { get; set; }
}