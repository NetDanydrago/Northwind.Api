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

internal class DesactivateProductController(IDesactivateProductInputPort inputPort) : IDesactiveProductController
{
    public async Task<HandlerRequestResult> DesactiveProductAsync(DesactivateProductDto desactivateProductDto)
    {
		HandlerRequestResult result = default;
		try
		{
			await inputPort.DesactivateAsync(desactivateProductDto);
			result = new HandlerRequestResult();
		}
		catch (Exception ex)
		{
			result = new HandlerRequestResult(ex.Message);
		}
		return result;
    }
}
