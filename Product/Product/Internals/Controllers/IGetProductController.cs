using Product.Dtos;
using Product.Dtos.ValueObjects;

namespace Product.Internals.Controllers;
internal interface IGetProductController
{
    Task<HandlerRequestResult<ProductDto>> GetProductByIdAsync(int id);
}