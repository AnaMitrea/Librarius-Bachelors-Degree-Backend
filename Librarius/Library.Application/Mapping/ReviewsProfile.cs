using AutoMapper;
using Library.Application.Models.Reviews;
using Library.DataAccess.Entities;

namespace Library.Application.Mapping;

public class ReviewsProfile : Profile
{
    public ReviewsProfile()
    {
        // DataAccess Entity -> Application Model

        CreateMap<Review, ReviewResponseModel>();
        CreateMap<ReviewsRequestModel, Review>();
    }
}