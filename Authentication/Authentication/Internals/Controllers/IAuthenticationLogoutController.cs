namespace Authentication.Internals.Controllers;
internal interface IAuthenticationLogoutController
{
    Task<HandlerRequestResult> LogoutAsync(string refreshToken);
}
