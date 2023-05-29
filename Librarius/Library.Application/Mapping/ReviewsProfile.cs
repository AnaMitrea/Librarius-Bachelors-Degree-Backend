using AutoMapper;
using Library.Application.Models.Reviews;
using Library.Application.Models.Reviews.Request;
using Library.Application.Utilities;
using Library.DataAccess.DTOs;
using Library.DataAccess.DTOs.User;
using Library.DataAccess.Entities.BookRelated;

namespace Library.Application.Mapping;

public class ReviewsProfile : Profile
{
    public ReviewsProfile()
    {
        // DataAccess Entity -> Application Model

        CreateMap<Review, ReviewModel>()
            .ForMember(dest => dest.TimeUnit,
                opt => 
                    opt.MapFrom(src => Utils.CalculateTimeUnit(src.Timestamp)))
            .ForMember(dest => dest.TimeValue,
                opt =>
                    opt.MapFrom(src => Utils.CalculateTimeValue(src.Timestamp)))
            .ForMember(dest => dest.Liked,
                opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.IsMyReview,
                opt =>
                    opt.MapFrom((src, dest, _, context) =>
                    {
                        var username = context.Items["Username"] as string;
                        return src.User.Username == username;
                    }));

        CreateMap<UserReviewRequestModel, UserReviewRequestDto>();
        
        CreateMap<ReviewRequestModel, Review>();
    }
}