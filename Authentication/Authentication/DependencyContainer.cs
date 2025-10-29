using Authentication.Controller;
using Authentication.Handler;
using Authentication.Options;
using Authentication.Services;

namespace Authentication;

public static class DependencyContainer
{
    public static IServiceCollection AddAuthCore(this IServiceCollection services, Action<JwtOptions> configureJwtOptions)
    {
        services.Configure(configureJwtOptions);
        // Register internal services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();

        // Register handlers
        services.AddScoped<IAuthenticationLoginInputPort, AuthenticationLoginHandler>();
        services.AddScoped<IAuthenticationRefreshTokenInputPort, AuthenticationRefreshTokenHandler>();
        services.AddScoped<IAuthenticationLogoutInputPort, AuthenticationLogoutHandler>();
        services.AddScoped<IAuthenticationRecoveryPasswordInputPort, AuthenticationRecoveryPasswordHandler>();

        // Register controllers
        services.AddScoped<IAuthenticationLoginController, AuthenticationLoginController>();
        services.AddScoped<IAuthenticationRefreshTokenController, AuthenticationRefreshTokenController>();
        services.AddScoped<IAuthenticationLogoutController, AuthenticationLogoutController>();
        services.AddScoped<IAuthenticationRecoveryPasswordController, AuthenticationRecoveryPasswordController>();

        return services;
    }
}
