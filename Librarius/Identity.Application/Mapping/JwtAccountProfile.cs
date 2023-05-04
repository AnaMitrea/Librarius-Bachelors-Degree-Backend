using AutoMapper;
using Identity.Application.Models.Requests;
using Identity.Application.Models.User;
using Identity.DataAccess.Entities;

namespace Identity.Application.Mapping;

public class JwtAccountProfile : Profile
{
    public JwtAccountProfile()
    {
        // DataAccess Entity -> Application Model
                
        // Authentication Only;
        CreateMap<Account, UserAccountModel>(); 
        CreateMap<Account, AuthenticationResponseModel>();
        CreateMap<Account, AuthJwtResponseModel>();
    }
}