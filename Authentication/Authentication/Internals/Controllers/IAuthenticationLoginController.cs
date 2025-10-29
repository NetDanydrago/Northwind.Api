namespace Authentication.Internals.Controllers;
internal interface IAuthenticationLoginController
{
    Task<HandlerRequestResult<AuthenticationDto>> LoginAsync(LoginDto loginDto);
}
