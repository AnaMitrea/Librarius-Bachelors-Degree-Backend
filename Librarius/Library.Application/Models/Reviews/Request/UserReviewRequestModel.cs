namespace Library.Application.Models.Reviews.Request;

public class UserReviewRequestModel
{
    public string ReviewContent { get; set; }
    
    public int BookId { get; set; }
    
    public int Rating { get; set; }
}