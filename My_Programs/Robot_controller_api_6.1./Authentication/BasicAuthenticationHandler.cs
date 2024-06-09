using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using robot_controller_api.Persistence;
using System.Net.Http.Headers;

namespace robot_controller_api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserDataAccess _userDataAccess;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserDataAccess userDataAccess
        )
            : base(options, logger, encoder, clock)
        {
            _userDataAccess = userDataAccess;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return AuthenticateResult.NoResult();
            }

            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            var authHeader = AuthenticationHeaderValue.Parse(
                Request.Headers[HeaderNames.Authorization]
            );
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            if (credentials.Length != 2)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            var email = credentials[0];
            var password = credentials[1];
            var user = _userDataAccess.GetUserByEmail(email);

            if (user == null)
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            try
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                return AuthenticateResult.Fail($"Password hash error: {ex.Message}");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
