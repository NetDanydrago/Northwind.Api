using Product.Dtos;
using Product.Dtos.ValueObjects;

namespace Product.Internals.Controllers;

internal interface IUpdateProductController
{
    Task<HandlerRequestResult> UpdateProductAsync(UpdateProductDto updateProductDto);
}
