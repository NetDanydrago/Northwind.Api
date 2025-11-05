using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IntegrationTests.Hosting;
public static class TestHostFactory
{
    public static IHost CreateDefaultHost(Action<IServiceCollection> configureService, Action<IEndpointRouteBuilder> addEndpoints)
    {
        IHostBuilder Builder = Host.CreateDefaultBuilder();
        Builder.ConfigureWebHost(builder =>
        {
            builder.UseTestServer()
            .ConfigureServices(services =>
            {
                configureService(services);
                services.AddRouting();
                services.AddAuthentication(defaultScheme: "TestScheme").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
                services.AddAuthorization(options =>
                {
                    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAssertion(context => true).Build();
                });
            })
            .Configure(App =>
            {
                App.UseRouting();
                App.UseAuthentication();
                App.UseAuthorization();
                App.UseEndpoints(builder => addEndpoints(builder));
            }); 
        });
        return Builder.Start();

    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
