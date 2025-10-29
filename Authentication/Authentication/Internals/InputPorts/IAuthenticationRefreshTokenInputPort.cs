namespace Authentication.Internals.InputPorts;
internal interface IAuthenticationRefreshTokenInputPort
{
    Task<AuthenticationDto> RefreshTokenAsync(string refreshToken);
}
