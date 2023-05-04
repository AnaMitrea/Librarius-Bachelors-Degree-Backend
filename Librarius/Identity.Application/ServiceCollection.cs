using Identity.Application.Mapping;
using Identity.Application.Services;
using Identity.Application.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class ServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(JwtAccountProfile));
        services.AddAutoMapper(typeof(UserProfile));
        
        services.AddScoped<IJwtTokenHandlerService, JwtTokenHandlerService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}