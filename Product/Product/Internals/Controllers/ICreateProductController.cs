using Product.Dtos;
using Product.Dtos.ValueObjects;

namespace Product.Internals.Controllers;
internal interface ICreateProductController
{
    Task<HandlerRequestResult> CreateProductAsync(CreateProductDto createProduct);
}
