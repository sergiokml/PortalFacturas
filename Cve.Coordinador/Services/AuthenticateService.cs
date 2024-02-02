using System.Text.Json;
using System.Text.Json.Nodes;

using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Services.Interfaces;

using Microsoft.Extensions.Configuration;

namespace Cve.Coordinador.Services
{
    internal class AuthenticateService : IAuthenticateService
    {
        private readonly IRepositoryBase repo;
        private readonly IConfiguration config;

        public AuthenticateService(IRepositoryBase repo, IConfiguration config)
        {
            this.repo = repo;
            this.config = config;
        }

        public async Task<string> Authenticate(CancellationToken ct)
        {
            string username = config.GetSection("CENConfig:User").Value!;
            string password = config.GetSection("CENConfig:Password").Value!;
            HttpResponseMessage? msg = await repo.PostJson(
                Properties.Coordinador.UrlTokenAuth,
                JsonSerializer.Serialize(new { username, password }),
                ct
            )!;
            await msg.EnsureSuccess();
            string body = await msg.Content.ReadAsStringAsync(ct);
            dynamic obj = JsonNode.Parse(body)!.AsObject();
            return (string)obj["token"];
        }
    }
}
