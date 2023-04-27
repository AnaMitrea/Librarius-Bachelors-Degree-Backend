﻿using AutoMapper;
using Identity.Application.Models.User;
using Identity.DataAccess.Entities;

namespace Identity.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // DataAccess Entity -> Application Model
        CreateMap<Account, UserAccountModel>(); // Only for Authentication

        CreateMap<Account, UserModel>();
    }
}