namespace Library.Application.Models.LibraryUser.Response;

public class UserLeaderboardByBooks : UserResponseModel
{
    public int Position { get; set; }
    
    public int NumberOfBooks { get; set; }
}