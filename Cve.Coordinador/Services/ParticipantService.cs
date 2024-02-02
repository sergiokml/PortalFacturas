using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

using Microsoft.AspNetCore.WebUtilities;

namespace Cve.Coordinador.Services
{
    internal class ParticipantService : IParticipantService
    {
        private readonly IRepositoryBase repo;
        private readonly JsonSerializerOptions options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public ParticipantService(IRepositoryBase repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<Participant>?> GetAll(CancellationToken ct)
        {
            Dictionary<string, string>? q = new() { ["limit"] = 1000.ToString() };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlParticipants, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg.Content.ReadFromJsonAsync<BaseModel<Participant>>(options, ct)
            )!.Results!;
        }

        public async Task<IEnumerable<Participant>?> GetById(int[] ids, CancellationToken ct)
        {
            Dictionary<string, string>? q =
                new() { ["id"] = string.Join(",", ids), ["limit"] = 1000.ToString() };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlParticipants, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg.Content.ReadFromJsonAsync<BaseModel<Participant>>(options, ct)
            )!.Results!;
        }

        public async Task<Participant?> GetById(int id, CancellationToken ct)
        {
            Dictionary<string, string>? q = new() { ["id"] = id.ToString() };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlParticipants, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg.Content.ReadFromJsonAsync<BaseModel<Participant>>(options, ct)
            )!.Results!.FirstOrDefault();
        }

        public async Task<Participant?> GetByRut(string rut, CancellationToken ct)
        {
            Dictionary<string, string>? q = new() { ["rut"] = rut };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlParticipants, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg.Content.ReadFromJsonAsync<BaseModel<Participant>>(options, ct)
            )!.Results!.FirstOrDefault();
        }
    }
}
