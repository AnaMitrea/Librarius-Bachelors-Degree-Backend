using AutoMapper;
using Identity.Application.Models.User.Activity;
using Identity.DataAccess.Entities;

namespace Identity.Application.Mapping;

public class LoginActivityProfile : Profile
{
    public LoginActivityProfile()
    {
        // DataAccess Entity -> Application Model
        CreateMap<LoginActivity, UserActivityResponseModel>();

    }
}