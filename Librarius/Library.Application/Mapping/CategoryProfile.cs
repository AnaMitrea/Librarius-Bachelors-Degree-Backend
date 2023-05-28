using AutoMapper;
using Library.Application.Models.Book.Explore.Category;
using Library.Application.Models.Category;
using Library.DataAccess.DTOs.Explore;
using Library.DataAccess.Entities.Library;

namespace Library.Application.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponseModel>();
        CreateMap<Category, CategoryWithBookshelfResponseModel>();

        CreateMap<Category, ExploreCategoryResponseModel>();
        CreateMap<ExploreCategoryResponseModel, ExploreCategoryDto>();
    }
}