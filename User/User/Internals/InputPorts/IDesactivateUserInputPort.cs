using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Dtos;

namespace User.Internals.InputPorts;

internal interface IDesactivateUserInputPort
{
    Task DesactivateUserAsync(DesactivateUserDto desactivateUserDto);
}
