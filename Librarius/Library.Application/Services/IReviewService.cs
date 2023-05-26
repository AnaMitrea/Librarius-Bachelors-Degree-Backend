using Library.Application.Models.Reviews.Request;
using Library.Application.Models.Reviews.Response;

namespace Library.Application.Services;

public interface IReviewService
{
    Task<RatingReviewsResponseModel> GetReviewsForBookByIdAsync(ReviewRequestModel requestModel, string username);
    
    Task<bool> SetUserReviewByBookIdAsync(UserReviewRequestModel requestModel, string username);
    
    Task<bool> DeleteReviewByIdAsync(int reviewId);

    Task<bool> UpdateLikeStatusAsync(string username, int reviewId, bool isLiked);
}