using AutoMapper;
using Library.Application.Models.LibraryUser;
using Library.Application.Models.LibraryUser.Response;
using Library.DataAccess.DTOs.User;
using Library.DataAccess.Entities;

namespace Library.Application.Mapping;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseModel>();
        
        CreateMap<User, UserLeaderboardDto>();
        CreateMap<User, UserLeaderboardByBooksDto>();
        CreateMap<User, UserLeaderboardByMinutesDto>();
        CreateMap<User, UserLeaderboardByPointsDto>();
        
        CreateMap<UserLeaderboardByBooksDto, UserLeaderboardByBooks>();
        CreateMap<UserLeaderboardByMinutesDto, UserLeaderboardByMinutes>();
        CreateMap<UserLeaderboardByPointsDto, UserLeaderboardByPoints>();
    }
}