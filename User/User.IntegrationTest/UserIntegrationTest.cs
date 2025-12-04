using IntegrationTests.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthWind.EFCore.Repositories;
using System.Net.Http.Json;
using User;
using User.Dtos;
using User.Dtos.ValueObjects;
using User.Rest.Mappings;

namespace User.IntegrationTest;

public class UserIntegrationTest
{
    [Fact]
    public async Task GetAllUsers_ReturnHandlerRequestResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(
            services =>
            {
                services.AddNorthWIndRepositoriesSqlLite();
                services.AddUserCore();
            },
            endpoints =>
            {
                endpoints.UseUserEndpoints();
            });

        using HttpClient client = host.GetTestClient();

        var http = await client.GetAsync("api/users");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<IEnumerable<UserDto>>>();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateUser_ReturnSuccessResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(
            services =>
            {
                services.AddNorthWIndRepositoriesSqlLite();
                services.AddUserCore();
            },
            endpoints =>
            {
                endpoints.UseUserEndpoints();
            });

        using HttpClient client = host.GetTestClient();

        var dto = new CreateUserDto("juan.perez", "Pérez López","juan.perez@test.com","P@ssw0rd123");

        var http = await client.PostAsJsonAsync("api/users", dto);
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUserById_ReturnsHandlerRequestResultOfUser()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(
            services =>
            {
                services.AddNorthWIndRepositoriesSqlLite();
                services.AddUserCore();
            },
            endpoints =>
            {
                endpoints.UseUserEndpoints();
            });

        using HttpClient client = host.GetTestClient();

        var createDto = new CreateUserDto("maria.garcia", "García Ruiz", "maria.garcia@test.com", "P@ssw0rd123");

        var createHttp = await client.PostAsJsonAsync("api/users", createDto);
        var createResult = await createHttp.Content.ReadFromJsonAsync<HandlerRequestResult>();
        Assert.NotNull(createResult);
        Assert.True(createResult.Success);

        var http = await client.GetAsync("api/users/1");
        Assert.True(http.IsSuccessStatusCode, $"Status: {http.StatusCode}");

        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<UserDto>>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateUser_DuplicateEmail_ReturnsFailureResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(services =>
        {
            services.AddNorthWIndRepositoriesSqlLite();
            services.AddUserCore();
        },
        endpoints =>
        {
            endpoints.UseUserEndpoints();
        });

        using HttpClient client = host.GetTestClient();

        var email = "juan.perez@test.com";

        var firstUser = new CreateUserDto("juan.perez", "Pérez López", email, "P@ssw0rd123");
        await client.PostAsJsonAsync("api/users", firstUser);

        var secondUser = new CreateUserDto("juan2", "Pérez López", email, "P@ssw0rd123");
        var http = await client.PostAsJsonAsync("api/users", secondUser);

        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateUser_ReturnsSuccessResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(
            services =>
            {
                services.AddNorthWIndRepositoriesSqlLite();
                services.AddUserCore();
            },
            endpoints =>
            {
                endpoints.UseUserEndpoints();
            });

        using HttpClient client = host.GetTestClient();

        var createDto = new CreateUserDto("carlos.romero", "Romero Díaz", "carlos.romero@test.com", "P@ssw0rd123");

        var createHttp = await client.PostAsJsonAsync("api/users", createDto);
        var createResult = await createHttp.Content.ReadFromJsonAsync<HandlerRequestResult>();

        var updateDto = new UpdateUserDto(1, "carlos.romero.actualizado", "Romero Díaz", "carlos.romero@test.com", null);
        var http = await client.PutAsJsonAsync("api/users", updateDto);
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult<UserDto>>();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeactivateUser_ReturnsSuccessResult()
    {
        using IHost host = TestHostFactory.CreateDefaultHost(
            services =>
            {
                services.AddNorthWIndRepositoriesSqlLite();
                services.AddUserCore();
            },
            endpoints =>
            {
                endpoints.UseUserEndpoints();
            });

        using HttpClient client = host.GetTestClient();

        var createDto = new CreateUserDto("desactivar.user", "Baja Lógica", "desactivar@test.com", "P@ssw0rd123");

        var createHttp = await client.PostAsJsonAsync("api/users", createDto);
        var createResult = await createHttp.Content.ReadFromJsonAsync<HandlerRequestResult>();
        Assert.NotNull(createResult);
        Assert.True(createResult.Success);

        var http = await client.DeleteAsync("api/users/1");
        var result = await http.Content.ReadFromJsonAsync<HandlerRequestResult>();

        Assert.NotNull(result);
    }
}
