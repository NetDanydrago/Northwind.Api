namespace Authentication.Controller;

internal class AuthenticationLoginController(IAuthenticationLoginInputPort inputPort) : IAuthenticationLoginController
{
    public async Task<HandlerRequestResult<AuthenticationDto>> LoginAsync(LoginDto loginDto)
    {
        HandlerRequestResult<AuthenticationDto> Result = default;
        try
        {
            var AutenticatioResult = await inputPort.LoginAsync(loginDto);
            Result = new HandlerRequestResult<AuthenticationDto>(new AuthenticationDto() { Token = AutenticatioResult.Token, User = AutenticatioResult.User });
        }
        catch (Exception ex)
        {
            Result = new HandlerRequestResult<AuthenticationDto>(ex.Message);
        }
        return Result;
    }
}
