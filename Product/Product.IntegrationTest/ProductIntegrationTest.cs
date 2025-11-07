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
    public class CreateProductDto(string name, string description, int categoryId)
    {
        public string Name => name;
        public string Description => description;
        public int CategoryId => categoryId;
    }

    public class UpdateProductDto(string name, string description, int categoryId)
    {
        public string Name => name;
        public string Description => description;
        public int CategoryId => categoryId;
    }

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
        Assert.True(result!.Success);
        Assert.True(string.IsNullOrWhiteSpace(result.ErrorMessage));
    }

    [Fact]
    public async Task GetProductById_ReturnsHandlerRequestResultOfProduct()
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
        var name = "Teclado";
        var dto = new CreateProductDto(name, "Nuevo", 1);
        await client.PostAsJsonAsync("api/products", dto);

        using var scope = host.Services.CreateScope();
        var queryRepo = scope.ServiceProvider.GetRequiredService<IQueryableProductRepository>();
        var created = await queryRepo.GetByNameAsync(name);
        Assert.NotNull(created);

        var http = await client.GetAsync($"api/products/{created!.Id}");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<ProductDto>>();

        Assert.NotNull(result);
        Assert.True(result!.Success);
        Assert.NotNull(result.SuccessValue);
        Assert.Equal(created.Id, result.SuccessValue!.Id);
        Assert.Equal(name, result.SuccessValue.Name);
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

        var dto = new CreateProductDto("ProductoActualizar", "Viejo", 1);

        using var scope = host.Services.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IQueryableProductRepository>();
        var created = await repo.GetByNameAsync("ProductoActualizar");
        Assert.NotNull(created);


        var update = new UpdateProductDto("ProductoActualizar", "Nuevo", 1);

        var http = await client.PutAsJsonAsync("api/products", update);
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
        Assert.True(result!.Success);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsSuccessResult()
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
        var dto = new CreateProductDto("ProductoEliminar", "Temporal", 1);

        using var scope = host.Services.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IQueryableProductRepository>();
        var created = await repo.GetByNameAsync("ProductoEliminar");
        Assert.NotNull(created);

        var http = await client.DeleteAsync($"api/products/{created!.Id}");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
        Assert.True(result!.Success);
    }
}
