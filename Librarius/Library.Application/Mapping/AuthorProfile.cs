using AutoMapper;
using Library.Application.Models.Book.Author;
using Library.DataAccess.Entities.BookRelated;

namespace Library.Application.Mapping;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorResponseModel>();
    }
}