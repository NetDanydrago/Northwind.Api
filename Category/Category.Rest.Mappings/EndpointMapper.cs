using Category.Dtos;
using Category.Dtos.ValueObjects;
using Category.Internals.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Category.Rest.Mappings;

public static class EndpointMapper
{
    public static IEndpointRouteBuilder UseCategoryEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapPost("api/categories", async (ICreateCategoryController controller, [FromBody] CreateCategoryDto createCategoryDto) =>
        {
            return TypedResults.Ok(await controller.CreateAsync(createCategoryDto));
        }).Produces<HandlerRequestResult<CategoryDto>>();

        builder.MapPut("api/categories", async (IUpdateCategoryController controller, [FromBody] UpdateCategoryDto updateCategoryDto) =>
        {
            return TypedResults.Ok(await controller.UpdateAsync(updateCategoryDto));
        }).Produces<HandlerRequestResult<CategoryDto>>();

        builder.MapGet("api/categories/{id:int}", async (IGetCategoryController controller, int id) =>
        {
            return TypedResults.Ok(await controller.GetByIdAsync(id));
        }).Produces<HandlerRequestResult<CategoryDto>>();

        builder.MapGet("api/categories", async (IGetCategoryController controller) =>
        {
            return TypedResults.Ok(await controller.GetAllActiveAsync());
        }).Produces<HandlerRequestResult<IEnumerable<CategoryDto>>>();

        builder.MapDelete("api/categories/{id:int}", async (IDeactivateCategoryController controller, int id) =>
        {
            var deactivateCategoryDto = new DeactivateCategoryDto(id);
            return TypedResults.Ok(await controller.DeactivateAsync(deactivateCategoryDto));
        }).Produces<HandlerRequestResult>();

        return builder;
    }
}