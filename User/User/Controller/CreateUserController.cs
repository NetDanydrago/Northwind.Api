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

internal class CreateUserController(ICreateUserInputPort inputPort) : ICreateUserController
{
    public async Task<HandlerRequestResult> CreateUserAsync(CreateUserDto createUserDto)
    {
        HandlerRequestResult result = default;
		try
		{
			await inputPort.CreateUserAsync(createUserDto);
			result = new HandlerRequestResult();
		}
		catch (Exception ex)
		{
			result = new HandlerRequestResult(ex.Message);
		}
		return result;
    }
}
