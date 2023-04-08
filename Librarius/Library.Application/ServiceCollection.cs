using Library.Application.Mapping;
using Library.Application.Services;
using Library.Application.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application;

public static class ServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(BookshelfProfile));
        services.AddAutoMapper(typeof(CategoryProfile));
        services.AddAutoMapper(typeof(BookProfile));
        services.AddAutoMapper(typeof(BookCategoryProfile));
        
        // TODO add services as:  services.AddScoped<IDogService, DogService>();
        services.AddScoped<IBookshelfService, BookshelfService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBookService, BookService>();

        // TODO models validators as:  services.AddScoped<IValidator<DogRequestModel>, DogRequestModelValidator>();
        
        
        return services;
    }
}