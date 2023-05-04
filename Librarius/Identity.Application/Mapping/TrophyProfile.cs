using AutoMapper;
using Identity.Application.Models.Trophy;
using Identity.DataAccess.Entities;

namespace Identity.Application.Mapping;

public class TrophyProfile : Profile
{
    public TrophyProfile()
    {
        CreateMap<Trophy, TrophyModel>();
    }
}