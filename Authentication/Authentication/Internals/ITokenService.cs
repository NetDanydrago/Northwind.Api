using Authentication.Dtos;

namespace Authentication.Internals;

internal interface ITokenService
{
    string GenerateAccessToken(UserDto user);
    RefreshTokenDto GenerateRefreshToken();
    ClaimsPrincipal ValidateToken(string token);
    bool IsTokenExpired(string token);
    DateTime GetTokenExpiration(string token);
}
