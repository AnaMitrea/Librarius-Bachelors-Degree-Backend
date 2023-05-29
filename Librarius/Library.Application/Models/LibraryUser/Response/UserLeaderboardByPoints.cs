namespace Library.Application.Models.LibraryUser.Response;

public class UserLeaderboardByPoints: UserResponseModel
{
    public int Position { get; set; }
    
    public int Points { get; set; }
}