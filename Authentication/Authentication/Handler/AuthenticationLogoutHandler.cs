namespace Authentication.Handler;
internal class AuthenticationLogoutHandler(ICommandRefreshTokenRepository refreshTokenRepository, ILogger<AuthenticationLogoutHandler> logger) : IAuthenticationLogoutInputPort
{
    public async Task LogoutAsync(string refreshToken)
    {
        try
        {
            logger.LogInformation("Attempting logout");
            await using var Scope = new DomainTransactionScope();
            await Scope.EnlistAsync(refreshTokenRepository);
            await refreshTokenRepository.RevokeTokenAsync(refreshToken);
            await refreshTokenRepository.SaveChangesAsync();
            Scope.Complete();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during logout");
            throw new Exception(AuthenticationMessages.LogoutError, ex);
        }
    }
}
