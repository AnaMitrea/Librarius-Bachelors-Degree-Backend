using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trophy.Application.Mapping;
using Trophy.Application.Services;
using Trophy.Application.Services.Implementations;

namespace Trophy.Application;

public static class ServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddAutoMapper(typeof(UserProfile));
        services.AddAutoMapper(typeof(TrophyProfile));
        
        services.AddSingleton<HttpClient>();
        
        // services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITrophyService, TrophyService>();

        return services;
    }
}