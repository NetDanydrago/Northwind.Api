namespace Authentication.Handler;

internal class AuthenticationLoginHandler(
    IQueryableAuthenticationUsersRepository userRepository,
    ICommandRefreshTokenRepository refreshTokenRepository,
    ITokenService tokenService,
    IPasswordService passwordService,
    ILogger<AuthenticationLoginHandler> logger) : IAuthenticationLoginInputPort
{
    public async Task<AuthenticationDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            logger.LogInformation("Attempting login for user: {Username}", loginDto.Username);

            var User = await userRepository.GetByUsernameAsync(loginDto.Username);
            if (User == null || !passwordService.VerifyPassword(loginDto.Password, User.PasswordHash))
            {
                throw new Exception(AuthenticationMessages.InvalidCredentials);
            }
            if (!User.IsActive)
            {
                throw new Exception(AuthenticationMessages.UserNotActive);
            }
            // Generate tokens
            var AccessToken = tokenService.GenerateAccessToken(User);
            var RefreshTokenValue = tokenService.GenerateRefreshToken();
            RefreshTokenValue.UserId = User.Id;
            await using var Scope = new DomainTransactionScope();
            await Scope.EnlistAsync(refreshTokenRepository);
            await refreshTokenRepository.CreateAsync(RefreshTokenValue);
            await refreshTokenRepository.SaveChangesAsync();
            Scope.Complete();

            logger.LogInformation("Successful login for user: {Username}", loginDto.Username);
            return new AuthenticationDto() { Token = new TokenDto() { AccessToken = AccessToken, RefreshToken = RefreshTokenValue.Token, ExpiresAt = tokenService.GetTokenExpiration(AccessToken) }, User = User };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during login for user: {Username}", loginDto.Username);
            throw;
        }
    }

}
