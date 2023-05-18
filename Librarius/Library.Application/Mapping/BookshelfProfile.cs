using AutoMapper;
using Library.Application.Models.Bookshelf;
using Library.DataAccess.Entities;
using Library.DataAccess.Entities.Library;

namespace Library.Application.Mapping;

public class BookshelfProfile : Profile
{
    public BookshelfProfile()
    {
        CreateMap<Bookshelf, BookshelfResponseModel>();
        
        CreateMap<Bookshelf, BookshelfWithCategoriesResponseModel>();
    }
}