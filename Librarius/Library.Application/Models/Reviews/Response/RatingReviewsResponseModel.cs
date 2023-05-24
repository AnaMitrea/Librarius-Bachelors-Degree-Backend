namespace Library.Application.Models.Reviews.Response;

public class RatingReviewsResponseModel
{
    public ICollection<ReviewModel> reviews { get; set; }

    public int overallRating { get; set; }
}