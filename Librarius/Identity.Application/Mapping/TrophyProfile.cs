using AutoMapper;
using Identity.Application.Models.Trophy;
using Identity.DataAccess.Entities;

namespace Identity.Application.Mapping;

public class TrophyProfile : Profile
{
    public TrophyProfile()
    {
        CreateMap<Trophy, TrophyModel>()
            .ForMember(dest => dest.IsWon, opt => opt.MapFrom(src => src.TrophyAccounts.Any(ta => ta.IsWon)));
    }
}