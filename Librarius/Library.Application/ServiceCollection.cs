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
        services.AddAutoMapper(typeof(AuthorProfile));
        services.AddAutoMapper(typeof(BookCategoryProfile));
        services.AddAutoMapper(typeof(UserProfile));
        services.AddAutoMapper(typeof(ReviewsProfile));
        services.AddAutoMapper(typeof(UserReadingBooksProfile));
        
        services.AddSingleton<HttpClient>();
        
        // Service Implementations
        services.AddScoped<IBookshelfService, BookshelfService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITriggerRewardService, TriggerRewardService>();

        return services;
    }
}