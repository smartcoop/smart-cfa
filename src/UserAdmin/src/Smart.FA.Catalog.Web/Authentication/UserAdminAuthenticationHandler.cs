using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Authentication
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CfaAuthenticationOptions>
    {
        private readonly IUserIdentity _userIdentity;
        public CustomAuthenticationHandler(
            IUserIdentity userIdentity,
            IOptionsMonitor<CfaAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _userIdentity = userIdentity;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (_userIdentity?.CurrentTrainer is null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Model is Empty"));
            }
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(_userIdentity.Identity), this.Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class CfaAuthenticationOptions
        : AuthenticationSchemeOptions
    { }
}
