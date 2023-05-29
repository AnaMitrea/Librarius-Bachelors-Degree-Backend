namespace Library.DataAccess.DTOs.User;

public class UserReviewRequestDto
{
    public string ReviewContent { get; set; }
    
    public int BookId { get; set; }
    
    public int Rating { get; set; }
}