using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

using Microsoft.AspNetCore.WebUtilities;

namespace Cve.Coordinador.Services
{
    internal class PeriodService : IPeriodService
    {
        private readonly IRepositoryBase repo;
        private readonly JsonSerializerOptions options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        private string Url()
        {
            return Properties.Coordinador.UrlPeriods;
        }

        public PeriodService(IRepositoryBase repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<Period>?> GetMany(object id, CancellationToken ct)
        {
            Dictionary<string, string>? query = new() { ["creditor"] = id.ToString()! };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Url(), query),
                ct
            )!;
            await msg.EnsureSuccess();
            return (await msg.Content.ReadFromJsonAsync<BaseModel<Period>>(options, ct))!.Results!;
        }
    }
}
