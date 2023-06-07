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
        services.AddAutoMapper(typeof(TrophyProfile));
        services.AddAutoMapper(typeof(LevelAssignProfile));
        
        services.AddSingleton<HttpClient>();
        
        services.AddScoped<ITrophyService, TrophyService>();
        services.AddScoped<ILevelAssignService, LevelAssignService>();

        return services;
    }
}