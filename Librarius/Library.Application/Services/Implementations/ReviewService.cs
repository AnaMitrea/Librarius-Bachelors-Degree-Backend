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
    
    public async Task<ICollection<ReviewResponseModel>> GetReviewsForBookByIdAsync(ReviewsRequestModel reviewsRequestModel)
    {
        var reviews = await _reviewsRepository.GetAllForBookByIdAsync(reviewsRequestModel.BookId);
        
        return _mapper.Map<ICollection<ReviewResponseModel>>(reviews);
    }
}