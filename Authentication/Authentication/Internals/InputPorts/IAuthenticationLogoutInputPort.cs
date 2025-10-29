namespace Authentication.Internals.InputPorts;
internal interface IAuthenticationLogoutInputPort
{
    Task LogoutAsync(string refreshToken);
}
