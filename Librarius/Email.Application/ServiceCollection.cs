using Email.Application.Services;
using Email.Application.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Email.Application;

public static class ServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
       // Service Implementations
        services.AddTransient<IEmailSender, EmailSender>(); // Transient instead of Scoped
        
        return services;
    }
}