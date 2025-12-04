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

internal class GetUserController(IGetUserInputPort inputPort) : IGetUserController
{
    public async Task<HandlerRequestResult<UserDto>> GetUserByIdAsync(int id)
    {
        HandlerRequestResult<UserDto> result = default;
		try
		{
			var userResult = await inputPort.GetUserByIdAsync(id);
			result = new HandlerRequestResult<UserDto>(userResult);
		}
		catch (Exception ex)
		{
			result = new HandlerRequestResult<UserDto>(ex.Message);
        }
		return result;
    }
}
