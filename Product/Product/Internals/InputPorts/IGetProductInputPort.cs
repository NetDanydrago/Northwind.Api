using Product.Dtos;

namespace Product.Internals.InputPorts;

internal interface IGetProductInputPort
{
    Task<ProductDto> GetProductByIdAsync(int id);
}
