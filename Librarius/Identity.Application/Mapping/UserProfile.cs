using AutoMapper;
using Identity.Application.Models.User;
using Identity.DataAccess.DTOs;
using Identity.DataAccess.Entities;
using UserModel = Identity.Application.Models.User.UserModel;

namespace Identity.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // DataAccess Entity -> Application Model
        
        CreateMap<Account, UserModel>();
        CreateMap<Account, DashboardUserModel>();
        
        CreateMap<Account, UserLeaderboardByPointsDto>();
        CreateMap<UserLeaderboardByPointsDto, UserLeaderboardByPoints>();
    }
}