
namespace Authentication.Controller;
internal class AuthenticationRecoveryPasswordController(IAuthenticationRecoveryPasswordInputPort inputPort) : IAuthenticationRecoveryPasswordController
{
    public async Task<HandlerRequestResult> RecoveryPasswordAsync(RecoveryPasswordDto recoveryPasswordDto)
    {
        HandlerRequestResult Result = new HandlerRequestResult();
        try
        { 
            await inputPort.RecoveryPasswordAsync(recoveryPasswordDto);
        }
        catch (Exception ex)
        {
            Result = new HandlerRequestResult(ex.Message);
        }
        return Result;
    }
}
