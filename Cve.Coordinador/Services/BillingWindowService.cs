using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

using Microsoft.AspNetCore.WebUtilities;

namespace Cve.Coordinador.Services
{
    internal class BillingWindowService : IBillingWindowService
    {
        private readonly IRepositoryBase repo;
        private readonly JsonSerializerOptions options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public BillingWindowService(IRepositoryBase repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<BillingWindow>?> GetByCreditor(
            int billing_type,
            DateTime period,
            int creditorId,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new()
                {
                    ["billing_type"] = billing_type.ToString(),
                    ["periods"] = period.ToString("yyyy-MM-dd"),
                    ["creditor"] = creditorId.ToString()
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlBillingWindows, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg.Content.ReadFromJsonAsync<BaseModel<BillingWindow>>(options, ct)
            )!.Results!;
        }
    }
}
