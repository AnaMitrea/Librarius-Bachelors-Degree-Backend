using Library.Application.Models.Reviews;

namespace Library.Application.Services;

public interface IReviewService
{
    Task<ICollection<ReviewResponseModel>> GetReviewsForBookByIdAsync(ReviewsRequestModel reviewsRequestModel);
}