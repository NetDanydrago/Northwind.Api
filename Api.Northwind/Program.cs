using Api.Northwind.Middleware;
using Category;
using Category.Rest.Mappings;
using User;
using User.Rest.Mappings;
using Product;
using Product.Rest.Mappings;
using NorthWind.EFCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddNorthWIndRepositoriesSqlLite();

builder.Services.AddCategoryCore();
builder.Services.AddProductCore();
builder.Services.AddUserCore();         

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
                          .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseProductEndpoints();
app.UseCategoryEndpoints();
app.UseUserEndpoints();
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseMiddleware<HandlerRequestResultMiddleware>();

app.InitializeSqlLiteDb();
app.Run();
