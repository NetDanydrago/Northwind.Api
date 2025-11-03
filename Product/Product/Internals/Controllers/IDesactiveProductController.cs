using Product.Dtos;
using Product.Dtos.ValueObjects;

namespace Product.Internals.Controllers;

internal interface IDesactiveProductController
{
    Task<HandlerRequestResult> DesactiveProductAsync(DesactivateProductDto desactivateProductDto);
}
