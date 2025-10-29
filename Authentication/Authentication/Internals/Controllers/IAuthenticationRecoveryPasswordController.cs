namespace Authentication.Internals.Controllers;
internal interface IAuthenticationRecoveryPasswordController
{
    Task<HandlerRequestResult> RecoveryPasswordAsync(RecoveryPasswordDto recoveryPasswordDto);
}
