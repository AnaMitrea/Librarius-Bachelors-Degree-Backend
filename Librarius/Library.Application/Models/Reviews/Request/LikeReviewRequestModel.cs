namespace Library.Application.Models.Reviews.Request;

public class LikeReviewRequestModel
{
    public int ReviewId { get; set; }
    
    public bool IsLiked { get; set; }
}