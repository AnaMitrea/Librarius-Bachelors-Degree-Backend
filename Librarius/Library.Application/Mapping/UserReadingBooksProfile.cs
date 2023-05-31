using AutoMapper;
using Library.Application.Models.Book.Reading.Response;
using Library.Application.Models.LibraryUser.Response;
using Library.DataAccess.Entities;
using Library.DataAccess.Entities.BookRelated;
using Library.DataAccess.Entities.User;

namespace Library.Application.Mapping;

public class UserReadingBooksProfile: Profile
{
    public UserReadingBooksProfile()
    {
        // DataAccess Level -> Application Level
        CreateMap<UserBookReadingTracker, ReadingTimeSpentResponseModel>()
            .ForMember(dest => dest.TimeSpent,
                o => 
                    o.MapFrom(urb => urb.MinutesSpent));

        CreateMap<UserBookReadingTracker, UserBookReadingTimeTrackerResponse>();
    }
}