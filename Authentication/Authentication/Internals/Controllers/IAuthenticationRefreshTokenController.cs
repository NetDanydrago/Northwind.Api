namespace Authentication.Internals.Controllers;
internal interface IAuthenticationRefreshTokenController
{
    Task<HandlerRequestResult<AuthenticationDto>> RefreshTokenAsync(string refreshToken);
}
