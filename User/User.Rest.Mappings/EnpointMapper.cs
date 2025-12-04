using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using User.Dtos;
using User.Dtos.ValueObjects;
using User.Internals.Controllers;

namespace User.Rest.Mappings;

public static class EndpointMapper
{
    public static IEndpointRouteBuilder UseUserEndpoints(this IEndpointRouteBuilder builder)
    {
        // POST: api/users
        builder.MapPost("api/users", async ( ICreateUserController controller, [FromBody] CreateUserDto createUserDto) =>
        {
            return TypedResults.Ok(await controller.CreateUserAsync(createUserDto));
        })
        .Produces<HandlerRequestResult<UserDto>>();

        // GET: api/users
        builder.MapGet("api/users", async (IGetAllUsersController controller) =>
        {
            return TypedResults.Ok(await controller.GetAllUsersActivateAsync());
        })
        .Produces<HandlerRequestResult<IEnumerable<UserDto>>>();

        // GET: api/users/{id:int}
        builder.MapGet("api/users/{id:int}", async (IGetUserController controller, int id) =>
        {
            return TypedResults.Ok(await controller.GetUserByIdAsync(id));
        })
        .Produces<HandlerRequestResult<UserDto>>();

        // PUT: api/users
        builder.MapPut("api/users", async ( IUpdateUserController controller, [FromBody] UpdateUserDto updateUserDto) =>
        {
            return TypedResults.Ok(await controller.UpdateUserAsync(updateUserDto));
        })
        .Produces<HandlerRequestResult<UserDto>>();

        // DELETE: api/users/{id:int}
        builder.MapDelete("api/users/{id:int}", async (
            IDesactivateUserController controller,
            int id) =>
        {
            var desactivateUserDto = new DesactivateUserDto(id);
            return TypedResults.Ok(await controller.DesactivateUserAsync(desactivateUserDto));
        })
        .Produces<HandlerRequestResult>();

        return builder;
    }
}
