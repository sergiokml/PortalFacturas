using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

using Microsoft.AspNetCore.WebUtilities;

namespace Cve.Coordinador.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly IRepositoryBase repo;
        private readonly JsonSerializerOptions options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public PaymentService(IRepositoryBase repo)
        {
            this.repo = repo;
        }

        public async Task<PaymentMatrix?> GetById(int id, CancellationToken ct)
        {
            Dictionary<string, string>? q = new() { ["id"] = id.ToString() };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlPaymentMatrices, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return JsonSerializer
                .Deserialize<BaseModel<PaymentMatrix>>(await msg.Content.ReadAsStreamAsync(ct))!
                .Results!.FirstOrDefault();
        }

        public async Task<IEnumerable<PaymentMatrix>?> GetByDate(
            DateTime publishdate,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new() { ["publish_date"] = publishdate.ToString("yyyy-MM-dd") };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlPaymentMatrices, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return JsonSerializer
                .Deserialize<BaseModel<PaymentMatrix>>(await msg.Content.ReadAsStreamAsync(ct))!
                .Results!;
        }

        public async Task<IEnumerable<PaymentMatrix>?> GetByPeriod(
            DateTime after,
            CancellationToken ct
        )
        {
            // LIMIT = HASTA 03-2023 HABÍAN 1100 PM EN TOTAL
            Dictionary<string, string>? q =
                new() { ["created_after"] = after.ToString("yyyy-MM-dd"), ["limit"] = "3000" };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlPaymentMatrices, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg.Content!.ReadFromJsonAsync<BaseModel<PaymentMatrix>>(options, ct)
            )!.Results!;
        }
    }
}
