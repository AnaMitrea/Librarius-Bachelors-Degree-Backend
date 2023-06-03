using Identity.DataAccess.Persistence;
using Identity.DataAccess.Repositories;
using Identity.DataAccess.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.DataAccess;

public static class ServiceCollection
{

    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(configuration["ConnectionString:DefaultConnection"]);
        });
        
        // add scoped repositories
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ILoginActivityRepository, LoginActivityRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}