using AutoMapper;
using Trophy.Application.Models;
using Trophy.Application.Models.Trophy.Response;

namespace Trophy.Application.Mapping;

public class TrophyProfile : Profile
{
    public TrophyProfile()
    {
        CreateMap<DataAccess.Entities.Trophy, TrophyModel>()
            .ForMember(dest => dest.IsWon, 
                opt
                    => opt.MapFrom(src => src.TrophyAccounts.Any(ta => ta.IsWon))
                    );
    }
}