using System.Diagnostics;
using AutoMapper;
using Library.Application.Models.Reviews;
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
            overallRating = 0,
            reviews = _mapper.Map<ICollection<ReviewModel>>(reviews)
        };

        return response;
    }
}