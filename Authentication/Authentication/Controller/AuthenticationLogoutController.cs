namespace Authentication.Controller;
internal class AuthenticationLogoutController(IAuthenticationLogoutInputPort inputPort) : IAuthenticationLogoutController
{
    public async Task<HandlerRequestResult> LogoutAsync(string refreshToken)
    {
        HandlerRequestResult Result = new HandlerRequestResult();
        try
        {
            await inputPort.LogoutAsync(refreshToken);
        }
        catch (Exception ex)
        {
            Result = new HandlerRequestResult(ex.Message);
        }
        return Result;
    }
}
