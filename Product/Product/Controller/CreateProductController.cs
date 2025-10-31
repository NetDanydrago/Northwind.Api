using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Product.Dtos;
using Product.Dtos.ValueObjects;
using Product.Internals.Controllers;
using Product.Internals.InputPorts;

namespace Product.Internals.Controllers;
internal class CreateProductController(ICreateProductInputPort inputPort) : ICreateProductController
{
    public async Task<HandlerRequestResult> CreateProductAsync(CreateProductDto createProductDto)
    {
        HandlerRequestResult result = default;
        try
        {
            await inputPort.CreateProductAsync(createProductDto);
            result = new HandlerRequestResult();
        }
        catch (Exception ex)
        {
            result = new HandlerRequestResult(ex.Message);
        }
        return result;
    }
}
   