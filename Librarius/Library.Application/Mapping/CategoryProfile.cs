using AutoMapper;
using Library.Application.Models.Category;
using Library.DataAccess.Entities;
using Library.DataAccess.Entities.Library;

namespace Library.Application.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponseModel>();
        CreateMap<Category, CategoryWithBookshelfResponseModel>();
    }
}