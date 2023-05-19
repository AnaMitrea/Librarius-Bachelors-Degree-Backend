using AutoMapper;
using Library.Application.Models.BookCategory;
using Library.DataAccess.Entities.Library;

namespace Library.Application.Mapping;

public class BookCategoryProfile : Profile
{
    public BookCategoryProfile()
    {
        CreateMap<BookCategory, BookCategoryResponseModel>()
            .ForMember(
                dest => dest.Title,
                dest 
                    => dest.MapFrom(source => source.Category.Title)
            );
    }
}