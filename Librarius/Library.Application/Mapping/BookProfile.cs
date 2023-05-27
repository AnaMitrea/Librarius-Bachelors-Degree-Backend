using AutoMapper;
using Library.Application.Models.Book;
using Library.Application.Models.Book.Explore.Bookshelf;
using Library.Application.Models.Book.Home;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Trending;
using Library.DataAccess.DTOs;
using Library.DataAccess.Entities.Library;

namespace Library.Application.Mapping;

public class BookProfile : Profile
{
    public BookProfile()
    {
        // DataAccess Entity -> Application Model

        CreateMap<Book, BookResponseModel>();

        CreateMap<Book, BookshelfBookResponseModel>();
        
        CreateMap<Book, BookNoCategoriesResponseModel>();
        
        CreateMap<BookWithContentDto, BookReadingResponseModel>();

        CreateMap<Dictionary<string, BookshelfWithBooksDto>, Dictionary<string, BooksForBookshelfResponseModel>>();
        CreateMap<Book, BookshelfBookResponseModel>();

        CreateMap<Book, BookTrendingResponseModel>();

        CreateMap<Book, ExploreBookResponseModel>();
    }
}