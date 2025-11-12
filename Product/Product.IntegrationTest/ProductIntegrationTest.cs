using IntegrationTests.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthWind.EFCore.Repositories;
using Product.Dtos;
using Product.Dtos.ValueObjects;
using Product.Interfaces;
using Product.Rest.Mappings;
using System.Net.Http.Json;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace Product.IntegrationTest;

public class ProductIntegrationTest
{
    [Fact]
    public async Task GetAllProduct_ReturnHandlerRequestResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost( services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddProductCore();
        }, endpoints =>
        {
            endpoints.UseProductEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var http = await client.GetAsync("api/products");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<IEnumerable<ProductDto>>>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateProduct_ReturnSuccesResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddProductCore();
        },
        enpoints =>
        {
            enpoints.UseProductEndpoints();
        });
        using HttpClient client = host.GetTestClient();
        var name = "Teclado Logitech";
        var dto = new CreateProductDto(name, "Nuevo", 1);
        var http = await client.PostAsJsonAsync("api/products", dto);
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetProductById_ReturnsHandlerRequestResultOfProduct()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddProductCore();
        }, endpoints =>
        {
            endpoints.UseProductEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var http = await client.GetAsync($"api/products/1");
        Assert.True(http.IsSuccessStatusCode, $"Status: {http.StatusCode}");

        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<ProductDto>>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateProduct_DuplicateName_ReturnsFailureResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddProductCore();
        },
        endpoints =>
        {
            endpoints.UseProductEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var name = "Mouse";

        await client.PostAsJsonAsync("api/products", new CreateProductDto(name, "Switches azules", 1));

        var http = await client.PostAsJsonAsync("api/products", new CreateProductDto(name, "Otra descripción", 1));

        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsSuccessResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddProductCore();
        },
        endpoints =>
        {
            endpoints.UseProductEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var update = new UpdateProductDto(2, "Actualizado", "Descripción actualizada", 1);
        var http = await client.PutAsJsonAsync("api/products", update);
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<ProductDto>>();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeactivateProduct_ReturnsSuccessResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddProductCore();
        },
        endpoints =>
        {
            endpoints.UseProductEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var name = "Mouse de Prueba";
        await client.PostAsJsonAsync("api/products", new CreateProductDto(name, "Prueba", 1));

        int id = 1;

        var http = await client.DeleteAsync($"api/products/{id}");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
    }
}
