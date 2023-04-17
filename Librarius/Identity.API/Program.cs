using Identity.Application;
using Identity.DataAccess;

const string myAllowAnyOrigin = "_myAllowAnyOrigin";
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
        });
});


// Add services to the container.

builder.Services.AddControllers();

// Identity.DataAccess configs
builder.Services.AddDataAccess(builder.Configuration);

// Identity.Application configs
builder.Services.AddApplication(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

app.UsePathBase(new PathString("/api"));
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myAllowAnyOrigin);

app.UseAuthorization();

app.MapControllers();

app.Run();