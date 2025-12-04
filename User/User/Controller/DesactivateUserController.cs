using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Dtos;
using User.Dtos.ValueObjects;
using User.Internals.Controllers;
using User.Internals.InputPorts;

namespace User.Controller;

internal class DesactivateUserController(IDesactivateUserInputPort inputPort) : IDesactivateUserController
{
    public async Task<HandlerRequestResult> DesactivateUserAsync(DesactivateUserDto desactivateUserDto)
    {
        HandlerRequestResult result = default;
		try
		{
			await inputPort.DesactivateUserAsync(desactivateUserDto);
			result = new HandlerRequestResult();
		}
		catch (Exception ex)
		{
			result = new HandlerRequestResult(ex.Message);
		}
		return result;
    }
}
