using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using PortalFacturas.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
            return (
                (dynamic)await (
                    await httpClient.PostAsJsonAsync("token-auth/", value)
                ).Content.ReadFromJsonAsync<TokenCen>()
            )?.Token;
        }

        public async Task<List<InstructionResult>> GetInstructionsAsync(
            string creditor,
            string debtor
        )
        {
            string requestUri = $"v2/resources/instructions/?creditor={creditor}&debtor={debtor}";
            try
            {
                InstructionModel instr = await httpClient.GetFromJsonAsync<InstructionModel>(
                    requestUri
                );
                return instr.Results.Where(c => c.Amount >= 10).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ParticipantResult>> GetParticipantsAsync(string username)
        {
            List<ParticipantResult> list = new();
            AgentResult agente = new();
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
            agente = (await httpClient.GetFromJsonAsync<AgentModel>(url)).Results.ToList()[0];

            List<int> ids = new List<int>();
            ids.AddRange(agente.Participants.Select(c => c.ParticipantID));
            string joined = string.Join(",", ids);

            //HttpResponseMessage res = await httpClient.GetAsync($"v1/resources/participants/?id={joined}");

            ParticipantModel response = await httpClient.GetFromJsonAsync<ParticipantModel>(
                $"v1/resources/participants/?id={joined}"
            );
            return response.Results.OrderBy(c => c.Name).ToList();



            //tareas = agente.Participants.Select(async m =>
            //{
            //    string requestUri = $"v1/resources/participants/?id={m.ParticipantID}";
            //    list.AddRange((await httpClient
            //    .GetFromJsonAsync<ParticipantModel>(requestUri)).Results);
            //    return list;
            //}).ToList();
            //await Task.WhenAll(tareas);
            //return list.OrderBy(c => c.Name).ToList();
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
