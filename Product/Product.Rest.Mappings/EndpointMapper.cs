using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Product.Dtos;
using Product.Dtos.ValueObjects;
using Product.Internals.Controllers;

namespace Product.Rest.Mappings;

public static class EndpointMapper
{
    public static IEndpointRouteBuilder UseProductEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/products", async ( ICreateProductController controller, [FromBody] CreateProductDto createProductDto) =>
        {
            return TypedResults.Ok(await controller.CreateProductAsync(createProductDto));
        })
        .Produces<HandlerRequestResult<ProductDto>>();

        builder.MapGet("api/products", async (IGetAllProductsController controller) =>
        {
            return TypedResults.Ok(await controller.GetAllProductActiveAsync());
        }).Produces<HandlerRequestResult<IEnumerable<ProductDto>>>();

        builder.MapGet("api/products/{id:int}", async (IGetProductController controller, int id) =>
        {
            return TypedResults.Ok(await controller.GetProductByIdAsync(id));
        }).Produces<HandlerRequestResult<ProductDto>>();

        builder.MapPut("api/products", async (IUpdateProductController controller, [FromBody] UpdateProductDto updateCategoryDto) =>
        {
            return TypedResults.Ok(await controller.UpdateProductAsync(updateCategoryDto));
        }).Produces<HandlerRequestResult<ProductDto>>();

        builder.MapDelete("api/products/{id:int}", async (IDesactiveProductController controller, int id) =>
        {
            var deactivateProductDto = new DesactivateProductDto(id);
            return TypedResults.Ok(await controller.DesactiveProductAsync(deactivateProductDto));
        }).Produces<HandlerRequestResult>();


        return builder;
    }
}
