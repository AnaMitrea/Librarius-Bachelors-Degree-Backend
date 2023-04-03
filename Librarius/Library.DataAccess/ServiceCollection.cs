using Library.DataAccess.Persistance;
using Library.DataAccess.Repositories;
using Library.DataAccess.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.DataAccess;

public static class ServiceCollection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(configuration["ConnectionString:DefaultConnection"]);
        });
        
        // add scoped repositories
        services.AddScoped<IBookshelfRepository, BookshelfRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();

        return services;
    }
}