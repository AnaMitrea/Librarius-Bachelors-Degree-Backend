using AutoMapper;
using Library.Application.Models.Bookshelf.Response;
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