namespace Authentication.Internals.InputPorts;
internal interface IAuthenticationLoginInputPort
{
    Task<AuthenticationDto> LoginAsync(LoginDto loginDto);
}
