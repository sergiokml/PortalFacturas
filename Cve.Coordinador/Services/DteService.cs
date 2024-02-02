using System.Net.Http.Json;

using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

using Microsoft.AspNetCore.WebUtilities;

namespace Cve.Coordinador.Services
{
    internal class DteService : IDteService
    {
        private readonly IRepositoryBase repo;

        public DteService(IRepositoryBase repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<Dte>> GetManyCreditorAsync(
            int creditor,
            int debtor,
            CancellationToken token
        )
        {
            Dictionary<string, string>? query =
                new()
                {
                    ["creditor"] = creditor.ToString(),
                    ["reported_by_creditor"] = "true",
                    ["debtor"] = debtor.ToString()
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlDtes, query),
                token
            )!;
            await msg.EnsureSuccess();
            return (await msg.Content.ReadFromJsonAsync<BaseModel<Dte>>())!.Results!;
        }

        public async Task<IEnumerable<Dte>> GetManyDebtorAsync(
            int idPm,
            int id,
            CancellationToken token
        )
        {
            Dictionary<string, string>? query =
                new()
                {
                    ["payment_matrix"] = idPm.ToString(),
                    ["reported_by_creditor"] = "true",
                    ["debtor"] = id.ToString()
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlDtes, query),
                token
            )!;
            await msg.EnsureSuccess();
            return (await msg.Content.ReadFromJsonAsync<BaseModel<Dte>>())!.Results!;
        }

        public async Task<IEnumerable<Dte>> GetManyInstructionId(int id, CancellationToken token)
        {
            Dictionary<string, string>? query =
                new() { ["instruction"] = id.ToString(), ["reported_by_creditor"] = "true" };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlDtes, query),
                token
            )!;
            await msg.EnsureSuccess();
            return (await msg.Content.ReadFromJsonAsync<BaseModel<Dte>>())!.Results!;
        }
    }
}
