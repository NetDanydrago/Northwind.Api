namespace Authentication.Controller;
internal class AuthenticationRefreshTokenController(IAuthenticationRefreshTokenInputPort inputPort) : IAuthenticationRefreshTokenController
{
    public async Task<HandlerRequestResult<AuthenticationDto>> RefreshTokenAsync(string refreshToken)
    {
        HandlerRequestResult<AuthenticationDto> Result = default;
        try
        {
            var AutenticatioResult = await inputPort.RefreshTokenAsync(refreshToken);
            Result = new HandlerRequestResult<AuthenticationDto>(new AuthenticationDto { Token = AutenticatioResult.Token, User = AutenticatioResult.User });
        }
        catch (Exception ex)
        {
            Result = new HandlerRequestResult<AuthenticationDto>(ex.Message);
        }
        return Result;
    }

}
