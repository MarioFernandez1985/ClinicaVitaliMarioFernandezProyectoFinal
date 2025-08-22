
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace ClinicaVitaliApi.Auth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _config;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            System.Text.Encodings.Web.UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration config) : base(options, logger, encoder, clock)
        {
            _config = config;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Autorizacion"))
                return Task.FromResult(AuthenticateResult.Fail("Informacion faltante "));

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Autorizacion"]);
                var bytes = Convert.FromBase64String(authHeader.Parameter ?? "");
                var pair = Encoding.UTF8.GetString(bytes).Split(':', 2);
                if (pair.Length != 2) return Task.FromResult(AuthenticateResult.Fail("Encabezado invalido"));

                var expectedUser = _config["Usuario"];
                var expectedPass = _config["Contraseña"];
                if (pair[0] != expectedUser || pair[1] != expectedPass)
                    return Task.FromResult(AuthenticateResult.Fail("Credenciales invalidos"));

                var claims = new[] { new Claim(ClaimTypes.Name, pair[0]) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}
