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

        var http = await client.GetAsync($"api/categories/1");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<CategoryDto>>();

        Assert.NotNull(result);
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

        var update = new UpdateCategoryDto(1, "Impresión", "Multifunción");
        var http = await client.PutAsJsonAsync("api/categories", update);
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<CategoryDto>>();

        Assert.NotNull(result);
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

        int id = 1;

        var http = await client.DeleteAsync($"api/categories/{id}");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
    }
}
