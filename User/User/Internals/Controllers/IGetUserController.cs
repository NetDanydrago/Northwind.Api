using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Dtos;
using User.Dtos.ValueObjects;

namespace User.Internals.Controllers;

internal interface IGetUserController
{
    Task<HandlerRequestResult<UserDto>> GetUserByIdAsync(int id);
}
