using AutoMapper;
using Library.Application.Models.Reviews;
using Library.Application.Utilities;
using Library.DataAccess.Repositories;

namespace Library.Application.Services.Implementations;

public class ReviewService: IReviewService
{
    private readonly IReviewsRepository _reviewsRepository;
    private readonly IMapper _mapper;

    public ReviewService(IReviewsRepository reviewsRepository, IMapper mapper)
    {
        _mapper = mapper;
        _reviewsRepository = reviewsRepository;
    }
    
    public async Task<RatingReviewsResponseModel> GetReviewsForBookByIdAsync(ReviewRequestModel reviewRequestModel)
    {
        var reviews = await _reviewsRepository.GetAllForBookByIdAsync(
            reviewRequestModel.BookId,
            reviewRequestModel.MaxResults,
            reviewRequestModel.SortBy,
            reviewRequestModel.StartIndex
        );
        
        var response = new RatingReviewsResponseModel
        {
            overallRating = Utils.CalculateOverallRating(reviews),
            reviews = _mapper.Map<ICollection<ReviewModel>>(reviews)
        };

        return response;
    }

    public async Task<bool> UpdateLikeStatusAsync(string username, int reviewId, bool isLiked)
    {
        var isSuccessful = await _reviewsRepository.UpdateLikeStatusAsync(username, reviewId, isLiked);
        
        return isSuccessful;
    }
}