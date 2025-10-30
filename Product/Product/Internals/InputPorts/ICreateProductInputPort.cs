using Product.Dtos;

namespace Product.Internals.InputPorts;
internal interface ICreateProductInputPort
{
    Task CreateProductAsync(CreateProductDto createProductDto);
}
