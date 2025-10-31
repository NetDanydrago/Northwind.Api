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

        return builder;
    }
}
