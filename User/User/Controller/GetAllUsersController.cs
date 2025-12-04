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

internal class GetAllUsersController(IGetAllUsersInputPort inputPort) : IGetAllUsersController
{
    public async Task<HandlerRequestResult<IEnumerable<UserDto>>> GetAllUsersActivateAsync()
    {
        HandlerRequestResult<IEnumerable<UserDto>> result = default;
		try
		{
			var userResult = await inputPort.GetAllUsersAsync();
			result = new HandlerRequestResult<IEnumerable<UserDto>>(userResult);
		}
		catch (Exception ex)
		{
			result = new HandlerRequestResult<IEnumerable<UserDto>>(ex.Message);
		}
		return result;
    }
}
