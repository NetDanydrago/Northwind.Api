using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Dtos;
using User.Dtos.ValueObjects;

namespace User.Internals.Controllers;

internal interface IDesactivateUserController
{
    Task<HandlerRequestResult> DesactivateUserAsync(DesactivateUserDto desactivateUserDto);
}
