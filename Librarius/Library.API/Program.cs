using FluentValidation.AspNetCore;
using Library.Application;
using Library.DataAccess;

const string myAllowAnyOrigin = "_myAllowAnyOrigin";
var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: myAllowAnyOrigin,
//         policy =>
//         {
//             policy.AllowAnyOrigin()
//                 .AllowAnyHeader()
//                 .AllowAnyMethod();
//         });
// });

builder.Services.AddFluentValidationAutoValidation();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Library.DataAccess configs
builder.Services.AddDataAccess(builder.Configuration);

// Library.Application configs
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

app.UsePathBase(new PathString("/api/library"));
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseSwagger().UseSwaggerUI(c => {
//         c.SwaggerEndpoint("library/swagger/v1/swagger.json", "Library.API V1");
// });

// app.UseCors(myAllowAnyOrigin);

app.UseAuthorization();

app.MapControllers();

app.Run();
