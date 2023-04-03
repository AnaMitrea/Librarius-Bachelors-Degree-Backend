using AutoMapper;
using Library.Application.Models.Bookshelf;
using Library.DataAccess.Entities;

namespace Library.Application.Mapping;

public class BookshelfProfile : Profile
{
    public BookshelfProfile()
    {
        CreateMap<Bookshelf, BookshelfResponseModel>();
    }
}