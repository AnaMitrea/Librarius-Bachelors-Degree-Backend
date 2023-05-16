using AutoMapper;
using Library.Application.Models.LibraryUser;
using Library.DataAccess.Entities;

namespace Library.Application.Mapping;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseModel>();
    }
}