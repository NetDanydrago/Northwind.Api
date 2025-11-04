using Product.Dtos;
using Product.Dtos.ValueObjects;
using Product.Internals.Controllers;
using Product.Internals.InputPorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Controller;
internal class GetProductController(IGetProductInputPort inputPort) : IGetProductController
{
    public async Task<HandlerRequestResult<ProductDto>> GetProductByIdAsync(int id)
    {
        HandlerRequestResult<ProductDto> result = default;
        try
        {
            var productResult = await inputPort.GetProductByIdAsync(id);
            result = new HandlerRequestResult<ProductDto>(productResult);
        }
        catch (Exception ex)
        {
            result = new HandlerRequestResult<ProductDto>(ex.Message);
        }
        return result;
    }
}
