using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Dtos.ValueObjects;

public class HandlerRequestResult
{
    public bool Success { get; init; }
    public string ErrorMessage { get; init; }

    public HandlerRequestResult()
    {
        Success = true;
        ErrorMessage = string.Empty;
    }

    public HandlerRequestResult(string message)
    {
        Success = false;
        ErrorMessage = message;
    }
}
