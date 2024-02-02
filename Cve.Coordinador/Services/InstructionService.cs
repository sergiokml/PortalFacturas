using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

using Microsoft.AspNetCore.WebUtilities;

namespace Cve.Coordinador.Services
{
    internal class InstructionService : IInstructionService
    {
        private readonly IRepositoryBase repo;
        private readonly JsonSerializerOptions options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public InstructionService(IRepositoryBase repo)
        {
            this.repo = repo;
        }

        public async Task<Instruction?> GetById(int id, CancellationToken ct)
        {
            Dictionary<string, string>? q =
                new() { ["id"] = id.ToString(), ["status"] = "Publicado" };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlInstruccionesv2, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg!.Content!.ReadFromJsonAsync<BaseModel<Instruction>>(options, ct)
            )!.Results!.FirstOrDefault();
        }

        public async Task<IEnumerable<Instruction>?> GetById(
            string creditorId,
            string debtorId,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new()
                {
                    ["creditor"] = creditorId,
                    ["debtor"] = debtorId,
                    ["status"] = "Publicado",
                    ["limit"] = "1000"
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlInstruccionesv2, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg!.Content!.ReadFromJsonAsync<BaseModel<Instruction>>(options, ct)
            )!.Results!;
        }

        public async Task<IEnumerable<Instruction>?> GetByCreditor(
            int pmId,
            int creditorId,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new()
                {
                    ["payment_matrix"] = pmId.ToString(),
                    ["creditor"] = creditorId.ToString(),
                    ["status"] = "Publicado",
                    ["limit"] = "1000"
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlInstruccionesv2, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg!.Content!.ReadFromJsonAsync<BaseModel<Instruction>>(options, ct)
            )!.Results!;
        }

        public async Task<int> GetByCreditor(int creditorId, CancellationToken ct)
        {
            Dictionary<string, string>? q =
                new() { ["creditor"] = creditorId.ToString(), ["status"] = "Publicado", };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlInstruccionesv2, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg!.Content!.ReadFromJsonAsync<BaseModel<Instruction>>(options, ct)
            )!.Count;
        }

        public async Task<IEnumerable<Instruction>?> GetByDebtor(
            int pmId,
            int debtorId,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new()
                {
                    ["payment_matrix"] = pmId.ToString(),
                    ["debtor"] = debtorId.ToString(),
                    ["status"] = "Publicado",
                    ["limit"] = "1000"
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlInstruccionesv2, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg!.Content!.ReadFromJsonAsync<BaseModel<Instruction>>(options, ct)
            )!.Results!;
        }

        public async Task<IEnumerable<Instruction>?> GetByCreditor(
            string[] debtors,
            string creditorId,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new()
                {
                    ["debtor_ids"] = string.Join(",", debtors),
                    ["creditor"] = creditorId,
                    ["limit"] = "1000",
                    ["status"] = "Publicado"
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlInstruccionesv1, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg!.Content!.ReadFromJsonAsync<BaseModel<Instruction>>(options, ct)
            )!.Results!;
        }

        public async Task<IEnumerable<Instruction>?> GetByDebtor(
            string[] creditors,
            string debtorId,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new()
                {
                    ["creditor_ids"] = string.Join(",", creditors),
                    ["debtor"] = debtorId,
                    ["limit"] = "1000",
                    ["status"] = "Publicado"
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlInstruccionesv1, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg!.Content!.ReadFromJsonAsync<BaseModel<Instruction>>(options, ct)
            )!.Results!;
        }

        public async Task<IEnumerable<Instruction>?> GetByCreditor(
            DateTime period,
            string creditorId,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new()
                {
                    ["creditor"] = creditorId,
                    ["period"] = period.Date.ToString("yyyy-MM-dd"),
                    ["limit"] = "1000",
                    ["status"] = "Publicado"
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlInstruccionesv2, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg!.Content!.ReadFromJsonAsync<BaseModel<Instruction>>(options, ct)
            )!.Results!;
        }

        public async Task<IEnumerable<Instruction>?> GetByDebtor(
            DateTime period,
            string debtorId,
            CancellationToken ct
        )
        {
            Dictionary<string, string>? q =
                new()
                {
                    ["debtor"] = debtorId,
                    ["period"] = period.Date.ToString("yyyy-MM-dd"),
                    ["limit"] = "1000",
                    ["status"] = "Publicado"
                };
            HttpResponseMessage? msg = await repo.GetJson(
                QueryHelpers.AddQueryString(Properties.Coordinador.UrlInstruccionesv1, q),
                ct
            )!;
            await msg.EnsureSuccess();
            return (
                await msg!.Content!.ReadFromJsonAsync<BaseModel<Instruction>>(options, ct)
            )!.Results!;
        }
    }
}
