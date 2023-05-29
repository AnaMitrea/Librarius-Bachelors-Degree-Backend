using AutoMapper;
using Library.Application.Models.Reviews;
using Library.Application.Models.Reviews.Request;
using Library.Application.Models.Reviews.Response;
using Library.Application.Utilities;
using Library.DataAccess.DTOs;
using Library.DataAccess.DTOs.User;
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
    
    public async Task<RatingReviewsResponseModel> GetReviewsForBookByIdAsync(
        ReviewRequestModel reviewRequestModel, string username)
    {
        var reviews = await _reviewsRepository.GetAllForBookByIdAsync(
            reviewRequestModel.BookId,
            reviewRequestModel.MaxResults,
            reviewRequestModel.SortBy,
            reviewRequestModel.StartIndex
        );
        
        var reviewModels = _mapper.Map<ICollection<ReviewModel>>(reviews, opts => opts.Items["Username"] = username);

        var response = new RatingReviewsResponseModel
        {
            overallRating = Utils.CalculateOverallRating(reviews),
            reviews = reviewModels
        };

        return response;
    }

    public async Task<bool> SetUserReviewByBookIdAsync(UserReviewRequestModel requestModel, string username)
    {
        return await _reviewsRepository.SetUserReviewByBookIdAsync(
            _mapper.Map<UserReviewRequestDto>(requestModel),
            username
        );
    }

    public async Task<bool> DeleteReviewByIdAsync(int reviewId)
    {
        return await _reviewsRepository.DeleteReviewByIdAsync(reviewId);
    }

    public async Task<bool> UpdateLikeStatusAsync(string username, int reviewId, bool isLiked)
    {
        return await _reviewsRepository.UpdateLikeStatusAsync(username, reviewId, isLiked);
    }
}