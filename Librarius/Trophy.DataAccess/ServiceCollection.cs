using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trophy.DataAccess.Persistence;
using Trophy.DataAccess.Repositories;
using Trophy.DataAccess.Repositories.Implementations;

namespace Trophy.DataAccess;

public static class ServiceCollection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(configuration["ConnectionString:DefaultConnection"]);
        });
        
        // add scoped repositories
        services.AddScoped<ITrophyRepository, TrophyRepository>();
        services.AddScoped<ILevelAssignRepository, LevelAssignRepository>();

        return services;
    }
}