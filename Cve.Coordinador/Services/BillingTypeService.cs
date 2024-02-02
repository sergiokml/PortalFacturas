using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

using Microsoft.AspNetCore.WebUtilities;

namespace Cve.Coordinador.Services
{
    internal class BillingTypeService : IBillingTypeService
    {
        private readonly IRepositoryBase repo;
        private readonly JsonSerializerOptions options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public BillingTypeService(IRepositoryBase repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<BillingType>?> GetByCreditor(
            DateTime period,
            int creditorId,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new()
                {
                    ["period"] = period.ToString("yyyy-MM-dd"),
                    ["creditor"] = creditorId.ToString()
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlBillingTypes, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg.Content.ReadFromJsonAsync<BaseModel<BillingType>>(options, ct)
            )!.Results!;
        }

        public async Task<BillingType>? GetById(string id, CancellationToken ct)
        {
            Dictionary<string, string>? q = new() { ["description_prefix"] = id! };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlBillingTypes, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg.Content.ReadFromJsonAsync<BaseModel<BillingType>>(options, ct)
            )!.Results![0]!;
        }
    }
}
