using AutoMapper;

namespace Email.Application.Mapping;

public class CustomProfile : Profile
{
    public CustomProfile()
    {
        // DataAccess -> Application
        // CreateMap<Author, AuthorResponseModel>();
        //
        // CreateMap<AuthorMaterialsDto, MaterialsResponseModel>()
        //     .ForMember(dest => dest.Title,
        // opt => opt.MapFrom(src => src.CategoryTitle)
        // );
    }
}