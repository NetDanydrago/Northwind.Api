using IntegrationTests.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NorthWind.EFCore.Repositories;
using Product.Dtos;
using Product.Dtos.ValueObjects;
using Product.Rest.Mappings;
using System.Net.Http.Json;

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
}
