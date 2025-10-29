using Api.Northwind.Middleware;
using Category;
using Category.Rest.Mappings;
using NorthWind.EFCore.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddNorthWIndRepositoriesSqlLite();
builder.Services.AddCategoryCore();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
                          .AllowAnyHeader());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCategoryEndpoints();
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
// Agregar el middleware personalizado para validar HandlerRequestResult
app.UseMiddleware<HandlerRequestResultMiddleware>();

app.InitializeSqlLiteDb();
app.Run();

