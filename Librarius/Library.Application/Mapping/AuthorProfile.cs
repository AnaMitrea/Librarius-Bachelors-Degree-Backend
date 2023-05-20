using AutoMapper;
using Library.Application.Models.Book.Author;
using Library.DataAccess.DTOs;
using Library.DataAccess.Entities.BookRelated;

namespace Library.Application.Mapping;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorResponseModel>();

        CreateMap<AuthorMaterialsDto, MaterialsResponseModel>()
            .ForMember(dest => dest.Title,
        opt => opt.MapFrom(src => src.CategoryTitle)
        );
    }
}