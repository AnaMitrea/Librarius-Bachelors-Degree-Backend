using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Email.Application;

public static class ServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        //Automappers
        // services.AddAutoMapper(typeof(BookshelfProfile));

        // Service Implementations
        // services.AddScoped<IBookshelfService, BookshelfService>();

        // TODO models validators as:  services.AddScoped<IValidator<DogRequestModel>, DogRequestModelValidator>();
        
        return services;
    }
}