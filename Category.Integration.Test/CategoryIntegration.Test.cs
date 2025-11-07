using System.Net.Http.Json;
using IntegrationTests.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthWind.EFCore.Repositories;
using Category.Dtos;
using Category.Dtos.ValueObjects;
using Category.Interfaces;
using Category.Rest.Mappings;
using Microsoft.AspNetCore.TestHost;

namespace Category.IntegrationTest;

public class CategoryIntegrationTest
{
    [Fact]
    public async Task GetAllCategories_ReturnHandlerRequestResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddCategoryCore();
        },
        endpoints =>
        {
            endpoints.UseCategoryEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var http = await client.GetAsync("api/categories");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<IEnumerable<CategoryDto>>>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateCategory_ReturnsSuccessResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddCategoryCore();
        },
        endpoints =>
        {
            endpoints.UseCategoryEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var dto = new CreateCategoryDto("Periféricos", "Teclados y mouse");
        var http = await client.PostAsJsonAsync("api/categories", dto);
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<CategoryDto>>();

        Assert.NotNull(result);
        Assert.True(result!.Success);
        Assert.True(string.IsNullOrWhiteSpace(result.ErrorMessage));
        Assert.NotNull(result.SuccessValue);
        Assert.Equal("Periféricos", result.SuccessValue.Name);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsHandlerRequestResultOfCategory()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddCategoryCore();
        },
        endpoints =>
        {
            endpoints.UseCategoryEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var name = "Audio";
        await client.PostAsJsonAsync("api/categories", new CreateCategoryDto(name, "Parlantes y audífonos"));

        using var scope = host.Services.CreateScope();
        var queryRepo = scope.ServiceProvider.GetRequiredService<IQueryableCategoryRepository>();
        var created = await queryRepo.GetByNameAsync(name);
        Assert.NotNull(created);

        var http = await client.GetAsync($"api/categories/{created!.Id}");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<CategoryDto>>();

        Assert.NotNull(result);
        Assert.True(result!.Success);
        Assert.NotNull(result.SuccessValue);
        Assert.Equal(created.Id, result.SuccessValue!.Id);
        Assert.Equal(name, result.SuccessValue.Name);
    }

    [Fact]
    public async Task CreateCategory_DuplicateName_ReturnsFailureResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddCategoryCore();
        },
        endpoints =>
        {
            endpoints.UseCategoryEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var name = "Almacenamiento";
        await client.PostAsJsonAsync("api/categories", new CreateCategoryDto(name, "SSD/HDD"));

        var http = await client.PostAsJsonAsync("api/categories", new CreateCategoryDto(name, "Duplicada"));
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<CategoryDto>>();

        Assert.NotNull(result);
        Assert.False(result!.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.ErrorMessage));
    }

    [Fact]
    public async Task UpdateCategory_ReturnsSuccessResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddCategoryCore();
        },
        endpoints =>
        {
            endpoints.UseCategoryEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var originalName = "Impresión";
        await client.PostAsJsonAsync("api/categories", new CreateCategoryDto(originalName, "Láser y tinta"));

        int id;
        using (var scope = host.Services.CreateScope())
        {
            var query = scope.ServiceProvider.GetRequiredService<IQueryableCategoryRepository>();
            id = (await query.GetByNameAsync(originalName))!.Id;
        }

        var update = new UpdateCategoryDto(id, "Impresión", "Multifunción");
        var http = await client.PutAsJsonAsync("api/categories", update);
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<CategoryDto>>();

        Assert.NotNull(result);
        Assert.True(result!.Success);
        Assert.Equal(id, result.SuccessValue!.Id);
        Assert.Equal("Impresión", result.SuccessValue.Name);
        Assert.Equal("Multifunción", result.SuccessValue.Description);
    }

    [Fact]
    public async Task DeactivateCategory_ReturnsSuccessResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddCategoryCore();
        },
        endpoints =>
        {
            endpoints.UseCategoryEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var name = "Redes";
        await client.PostAsJsonAsync("api/categories", new CreateCategoryDto(name, "Routers y switches"));

        int id;
        using (var scope = host.Services.CreateScope())
        {
            var query = scope.ServiceProvider.GetRequiredService<IQueryableCategoryRepository>();
            id = (await query.GetByNameAsync(name))!.Id;
        }

        var http = await client.DeleteAsync($"api/categories/{id}");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
        Assert.True(result!.Success);
    }
}
