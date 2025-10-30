using Product.Dtos;

namespace Product.Internals.InputPorts;
internal interface ICreateProductInputPort
{
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
}
