using AutoMapper;
using Trophy.Application.Models.LevelAssign.Response;
using Trophy.DataAccess.Entities;

namespace Trophy.Application.Mapping;

public class LevelAssignProfile : Profile
{
    public LevelAssignProfile()
    {
        // Data Access -> Application
        CreateMap<Level, LevelModel>();
    }
}