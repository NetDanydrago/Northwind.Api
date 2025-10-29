namespace Authentication.Internals.InputPorts;
internal interface IAuthenticationRecoveryPasswordInputPort
{
    Task RecoveryPasswordAsync(RecoveryPasswordDto recoveryPasswordDto);
}
