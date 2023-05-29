using AutoMapper;
using Library.Application.Models.Book.Reading.Response;
using Library.DataAccess.Entities.BookRelated;

namespace Library.Application.Mapping;

public class UserReadingBooksProfile: Profile
{
    public UserReadingBooksProfile()
    {
        // DataAccess Level -> Application Level
        CreateMap<UserReadingBooks, ReadingTimeSpentResponseModel>()
            .ForMember(dest => dest.TimeSpent,
                o => 
                    o.MapFrom(urb => urb.MinutesSpent));
    }
}