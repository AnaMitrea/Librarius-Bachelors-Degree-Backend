using Library.Application.Models.Reviews;

namespace Library.Application.Services;

public interface IReviewService
{
    Task<RatingReviewsResponseModel> GetReviewsForBookByIdAsync(ReviewRequestModel reviewRequestModel);
}