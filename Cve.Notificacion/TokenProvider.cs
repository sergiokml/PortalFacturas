using System.Text.Json;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Cve.Notificacion
{
    internal class TokenProvider : IAccessTokenProvider
    {
        private readonly IPublicClientApplication publicClientApplication;
        private AuthenticationResult Authentication { get; set; } = null!;

        public AllowedHostsValidator AllowedHostsValidator { get; set; } =
            new AllowedHostsValidator();

        private readonly IConfiguration config;
        private ILogger logger;

        public TokenProvider(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
            publicClientApplication = PublicClientApplicationBuilder
                .Create(config.GetSection("ADConfig:ClientId").Value!)
                .WithTenantId(config.GetSection("ADConfig:TenantId").Value!)
                .Build();
        }

        public async Task<string> GetAuthorizationTokenAsync(
            Uri uri,
            Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default
        )
        {
            string[] scopes = new[] { "https://graph.microsoft.com/.default" };
            if (Authentication == null || Authentication.ExpiresOn.UtcDateTime < DateTime.UtcNow)
            {
                Authentication = await publicClientApplication
                    .AcquireTokenByUsernamePassword(
                        scopes,
                        config.GetSection("EmailConfig:User").Value!,
                        config.GetSection("EmailConfig:Password").Value
                    )
                    .ExecuteAsync();
                logger.LogWarning(
                    $"Token expira en : {Authentication.ExpiresOn:dd-MM-yyyy HH:mm:ss} / Escope: {string.Join(" ", Authentication.Scopes)}"
                );
            }
            return Authentication.AccessToken!;
        }
    }
}
