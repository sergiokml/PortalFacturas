using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

internal class AgentService : IAgentService
{
    private readonly IRepositoryBase repo;
    private readonly IConfiguration config;
    private readonly JsonSerializerOptions options =
        new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public AgentService(IRepositoryBase repo, IConfiguration config)
    {
        this.repo = repo;
        this.config = config;
    }

    public async Task<IEnumerable<Agent>> GetByEmail(string email, CancellationToken ct)
    {
        email ??= config.GetSection("CENConfig:User").Value!;
        Dictionary<string, string>? q = new() { ["email"] = email };
        HttpResponseMessage? msg = await repo.GetJson(
            QueryHelpers.AddQueryString(Cve.Coordinador.Properties.Coordinador.UrlAgents, q),
            ct
        )!;
        await msg.EnsureSuccess();
        return (await msg.Content.ReadFromJsonAsync<BaseModel<Agent>>(options, ct))!.Results!;
    }
}
