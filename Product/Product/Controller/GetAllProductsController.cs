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

internal class GetAllProductsController(IGetAllProductsInputPort inputPort) : IGetAllProductsController
{
    public async Task<HandlerRequestResult<IEnumerable<ProductDto>>> GetAllProductActiveAsync()
    {
        HandlerRequestResult<IEnumerable<ProductDto>> result = default;
        try
        {
            var productsResult = await inputPort.GetAllProductsAsync();
            result = new HandlerRequestResult<IEnumerable<ProductDto>>(productsResult);
        }
        catch (Exception ex)
        {
            result = new HandlerRequestResult<IEnumerable<ProductDto>>(ex.Message);
        }
        return result;
    }
}
