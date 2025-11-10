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

        HttpResponseMessage response = await client.GetAsync("api/products");
        var result = await response.Content.ReadFromJsonAsync<HandlerRequestResult<IEnumerable<ProductDto>>>();
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

        var dto = new CreateProductDto("Teclado Logitech", "Nuevo", 1);
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
        Assert.False(result!.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.ErrorMessage));
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

        var update = new UpdateProductDto(1,"ProductoActualizar", "Nuevo", 1);

        var http = await client.PutAsJsonAsync("api/products", update);
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsSuccessResult()
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

    var http = await client.DeleteAsync("api/products/1"); 
    Assert.True(
        http.StatusCode == System.Net.HttpStatusCode.OK ||
        http.StatusCode == System.Net.HttpStatusCode.NoContent,
        $"Status: {http.StatusCode}");

    var raw = await http.Content.ReadAsStringAsync();
    if (!string.IsNullOrWhiteSpace(raw))
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<HandlerRequestResult>(raw);
            Assert.NotNull(result);
        }
    }
}
