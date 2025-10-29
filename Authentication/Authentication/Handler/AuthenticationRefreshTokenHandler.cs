namespace Authentication.Handler;
internal class AuthenticationRefreshTokenHandler(ICommandRefreshTokenRepository refreshTokenRepository, IQueryableAuthenticationUsersRepository userRepository, IQueryableRefreshTokenRepository queryableRefreshTokenRepository, ITokenService tokenService, ILogger<AuthenticationRefreshTokenHandler> logger) : IAuthenticationRefreshTokenInputPort
{
    public async Task<AuthenticationDto> RefreshTokenAsync(string refreshTokenDto)
    {
        try
        {
            logger.LogInformation("Attempting token refresh  {RefreshTokenDto}", refreshTokenDto);

            var RefreshToken = await queryableRefreshTokenRepository.GetByTokenAsync(refreshTokenDto);
            if (RefreshToken == null || RefreshToken.IsRevoked || RefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception(AuthenticationMessages.InvalidRefreshToken);
            }
            var User = await userRepository.GetByUserIdAsync(RefreshToken.UserId);
            if (User == null || !User.IsActive)
            {
                throw new Exception(AuthenticationMessages.UserNotFound);
            }
            // Generate new tokens
            var NewAccessToken = tokenService.GenerateAccessToken(User);
            var NewRefreshTokenValue = tokenService.GenerateRefreshToken();
            NewRefreshTokenValue.UserId = User.Id;
            await using var Scope = new DomainTransactionScope();
            await Scope.EnlistAsync(refreshTokenRepository);
            // Revoke old token
            RefreshToken.IsRevoked = true;
            RefreshToken.RevokedAt = DateTime.UtcNow;
            RefreshToken.ReplacedByToken = NewRefreshTokenValue.Token;
            await refreshTokenRepository.UpdateAsync(RefreshToken);
            await refreshTokenRepository.CreateAsync(NewRefreshTokenValue);
            await refreshTokenRepository.SaveChangesAsync();
            Scope.Complete();
            logger.LogInformation("Successful token refresh for user: {UserId}", User.Id);
            return new AuthenticationDto() { Token = new TokenDto() { AccessToken = NewAccessToken, RefreshToken = NewRefreshTokenValue.Token, ExpiresAt = tokenService.GetTokenExpiration(NewAccessToken) }, User = User };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during token refresh");
            throw;
        }
    }
}
