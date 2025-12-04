using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Dtos.ValueObjects;
public class HandlerRequestResult<T>
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public T SuccessValue { get; set; }

    public HandlerRequestResult()
    {

    }

    public HandlerRequestResult(string errorMessage)
    {
        Success = false;
        ErrorMessage = errorMessage;
    }

    public HandlerRequestResult(T result)
    {
        Success = true;
        SuccessValue = result;
    }
}