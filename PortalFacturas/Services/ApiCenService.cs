using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using PortalFacturas.Models;

namespace PortalFacturas.Services
{
    public interface IApiCenService
    {
        Task<List<ParticipantResult>> GetParticipantsAsync(string username = null);
        Task<string> GetAccessTokenAsync(string username, string password);
        Task<List<InstructionResult>> GetInstructionsAsync(string creditor, string debtor);
        Task GetDocumentos(List<InstructionResult> instructions);
    }

    public class ApiCenService : IApiCenService
    {
        private readonly HttpClient httpClient;
        private readonly AppSettings options;

        public ApiCenService(HttpClient httpClient, IOptions<AppSettings> options)
        {
            this.httpClient = httpClient;
            this.options = options.Value;
        }

        public async Task<string> GetAccessTokenAsync(string username, string password)
        {
            var value = new { Username = username, Password = password };
            HttpContent response = (await httpClient.PostAsJsonAsync("token-auth/", value)).Content;
            string body = await response.ReadAsStringAsync();
            dynamic obj = JsonNode.Parse(body).AsObject();
            return (string)obj["token"];
        }

        public async Task<List<InstructionResult>> GetInstructionsAsync(
            string creditor,
            string debtor
        )
        {
            string requestUri = $"v2/resources/instructions/?creditor={creditor}&debtor={debtor}";

            InstructionModel instr = await httpClient.GetFromJsonAsync<InstructionModel>(
                requestUri
            );
            return instr.Results.Where(c => c.Amount >= 10).ToList();
        }

        public async Task<List<ParticipantResult>> GetParticipantsAsync(string username)
        {
            List<ParticipantResult> list = new();
            List<Task<List<ParticipantResult>>> tareas = new();
            string url;
            if (username == null)
            {
                url = "v1/resources/agents/?limit=1000&email=" + options.EmailEmisor;
            }
            else
            {
                url = "v1/resources/agents/?limit=1000&email=" + username;
            }
            AgentResult agente = (
                await httpClient.GetFromJsonAsync<AgentModel>(url)
            ).Results.ToList()[0];

            List<int> ids = new();
            ids.AddRange(agente.Participants.Select(c => c.ParticipantID));
            string joined = string.Join(",", ids);
            ParticipantModel response = await httpClient.GetFromJsonAsync<ParticipantModel>(
                $"v1/resources/participants/?id={joined}"
            );
            return response.Results.OrderBy(c => c.BusinessName).ToList();
        }

        public async Task GetDocumentos(List<InstructionResult> instructions)
        {
            List<Task> tareas = new();
            tareas = instructions
                .Select(
                    async m =>
                    {
                        string requestUri =
                            $"v1/resources/dtes/?reported_by_creditor=true&instruction={m.Id}";
                        List<DteResult> dte = (
                            await httpClient.GetFromJsonAsync<DteModel>(requestUri)
                        ).Results.ToList();
                        if (dte != null && dte.Count > 0)
                        {
                            foreach (DteResult item in dte)
                            {
                                if (item.Type == 2) // 61 NC
                                {
                                    item.NetAmount *= -1;
                                }
                            }
                            m.DteResult = dte;
                        }
                    }
                )
                .ToList();
            await Task.WhenAll(tareas);
        }
    }
}
