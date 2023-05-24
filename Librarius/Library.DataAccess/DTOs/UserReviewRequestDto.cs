namespace Library.DataAccess.DTOs;

public class UserReviewRequestDto
{
    public string ReviewContent { get; set; }
    
    public int BookId { get; set; }
    
    public int Rating { get; set; }
}