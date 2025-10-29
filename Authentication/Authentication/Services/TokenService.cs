using Authentication.Options;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices.Marshalling;

namespace Authentication.Services;

internal class TokenService(IOptions<JwtOptions> options, ILogger<TokenService> logger) : ITokenService
{
    public string GenerateAccessToken(UserDto user)
    {
        var SecretKey = options.Value?.SecretKey ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

        var Claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role),
            new("FullName", user.FullName)
        };

        var Token = new JwtSecurityToken(
            issuer: options.Value?.ValidIssuer,
            audience: options.Value?.ValidAudience,
            claims: Claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(options.Value?.ExpirationMinutes ?? "60")),
            signingCredentials: Credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(Token);
    }

    public RefreshTokenDto GenerateRefreshToken()
    {
        var RandomBytes = new byte[32];
        using var Rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        Rng.GetBytes(RandomBytes);
        string RefreshTokenValue = Convert.ToBase64String(RandomBytes);
        // Save refresh token
        var RefreshToken = new RefreshTokenDto
        {
            Token = RefreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7), // 7 days for refresh token
            CreatedAt = DateTime.UtcNow
        };
        return RefreshToken;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var SecretKey = options.Value?.SecretKey ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        var ValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = options.Value?.ValidIssuer,
            ValidAudience = options.Value?.ValidAudience,
            IssuerSigningKey = Key,
            ClockSkew = TimeSpan.Zero
        };

        var TokenHandler = new JwtSecurityTokenHandler();
        return TokenHandler.ValidateToken(token, ValidationParameters, out _);
    }

    public bool IsTokenExpired(string token)
    {
        bool Result = false;
        try
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var JsonToken = TokenHandler.ReadJwtToken(token);
            Result = JsonToken.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            Result = true;
        }
        return Result;
    }

    public DateTime GetTokenExpiration(string token)
    {
        var TokenHandler = new JwtSecurityTokenHandler();
        var JsonToken = TokenHandler.ReadJwtToken(token);
        return JsonToken.ValidTo;
    }
}
