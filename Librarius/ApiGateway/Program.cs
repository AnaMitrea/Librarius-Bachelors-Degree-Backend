using Identity.Application.Services;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

const string myAllowAnyOrigin = "_CorsPolicy";
var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowAnyOrigin,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});


builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Identity.Application JWT Token Config Service
builder.Services.AddCustomJwtAuthentication();
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors(myAllowAnyOrigin);

app.UseAuthentication();

app.UseAuthorization();

app.UseWebSockets();
await app.UseOcelot();

app.Run();
