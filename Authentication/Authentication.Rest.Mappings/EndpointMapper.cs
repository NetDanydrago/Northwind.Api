using Authentication.Dtos;
using Authentication.Dtos.ValueObjects;
using Authentication.Internals.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Authentication.Rest.Mappings;

public static class EndpointMapper
{
    public static IEndpointRouteBuilder UseAuthEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapPost("api/auth/login", async (IAuthenticationLoginController controller, [FromBody] LoginDto loginDto) =>
        {
            return TypedResults.Ok(await controller.LoginAsync(loginDto));
        }).Produces<HandlerRequestResult>(); ;

        builder.MapPost("api/auth/refresh-token", async (IAuthenticationRefreshTokenController controller, [FromBody] string refreshToken) =>
        {
            return TypedResults.Ok(await controller.RefreshTokenAsync(refreshToken));
        }).Produces<HandlerRequestResult>();

        builder.MapPost("api/auth/logout", async (IAuthenticationLogoutController controller, [FromBody] string refreshToken) =>
        {
            return TypedResults.Ok(await controller.LogoutAsync(refreshToken));
        }).Produces<HandlerRequestResult>();

        builder.MapPost("api/auth/recover-password", async (IAuthenticationRecoveryPasswordController controller, [FromBody] RecoveryPasswordDto recoveryPassword ) =>
        {
           return TypedResults.Ok(await controller.RecoveryPasswordAsync(recoveryPassword));
        }).Produces<HandlerRequestResult>();

        return builder;
    }
}
