using AutoMapper;
using Library.Application.Models.Book;
using Library.DataAccess.DTOs;
using Library.DataAccess.Entities;

namespace Library.Application.Mapping;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookResponseModel>();
        
        CreateMap<BookWithContent, BookReadingResponseModel>();
        
        CreateMap<Book, BookTrendingResponseModel>();
    }
}