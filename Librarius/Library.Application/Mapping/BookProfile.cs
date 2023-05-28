using AutoMapper;
using Library.Application.Models.Book;
using Library.Application.Models.Book.Explore.Bookshelf;
using Library.Application.Models.Book.Explore.Category;
using Library.Application.Models.Book.Home;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Trending;
using Library.DataAccess.DTOs;
using Library.DataAccess.DTOs.Explore;
using Library.DataAccess.Entities.Library;

namespace Library.Application.Mapping;

public class BookProfile : Profile
{
    public BookProfile()
    {
        // DataAccess Entity -> Application Model

        CreateMap<Book, BookResponseModel>();

        // Books grouped by bookshelves
        CreateMap<BookshelfWithBooksDto, BooksForBookshelfResponseModel>();
        
        // Books grouped by bookshelves and categories
        CreateMap<BookshelfCategoryWithBooksDto, BooksForCategoryResponseModel>()
            .ForMember(
                dest => dest.Categories,
                opt => 
                    opt.MapFrom(src => src.Categories)
            );

        CreateMap<Book, BookshelfBookResponseModel>();

        CreateMap<Book, BookNoCategoriesResponseModel>();
        
        CreateMap<BookWithContentDto, BookReadingResponseModel>();
        
        CreateMap<Book, BookshelfBookResponseModel>();

        CreateMap<Book, BookTrendingResponseModel>();

        CreateMap<Book, ExploreBookResponseModel>();
    }
}