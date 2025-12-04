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

internal class UpdateUserController(IUpdateUserInputPort inputPort) : IUpdateUserController
{
    public async Task<HandlerRequestResult> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        HandlerRequestResult result = default;
		try
		{
			await inputPort.UpdateUserAsync(updateUserDto);
			result = new HandlerRequestResult();
		}
		catch (Exception ex)
		{
			result = new HandlerRequestResult(ex.Message);
		}
		return result;
    }
}
