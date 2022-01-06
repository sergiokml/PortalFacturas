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
        Task<InstructionModel> GetInstructionsAsync(string creditor, string debtor);
        Task<string> ConvertDocument(string filepath);
        Task GetDocumentos(List<InstructionResult> instructions);

    }

    public class ApiCenService : IApiCenService
    {
        private readonly HttpClient httpClient;
        //private readonly JsonSerializerOptions jsonSerializerOptions;
        private readonly OptionsModel options;

        //public IConfiguration Configuration { get; }

        public ApiCenService(HttpClient httpClient, IOptions<OptionsModel> options)
        {
            //Configuration = configuration;
            this.httpClient = httpClient;
            this.options = options.Value;

            //jsonSerializerOptions = new JsonSerializerOptions
            //{
            //    IgnoreNullValues = true
            //    //,DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            //};
        }

        public async Task<string> GetAccessTokenAsync(string username, string password)
        {
            var value = new
            {
                Username = username,
                Password = password
            };

            return ((dynamic)await (await httpClient.PostAsJsonAsync("token-auth/", value)).Content.ReadFromJsonAsync<TokenCen>())?.Token;
        }

        public async Task<InstructionModel> GetInstructionsAsync(string creditor, string debtor)
        {
            string requestUri = $"v2/resources/instructions/?creditor={creditor}&debtor={debtor}";
            try
            {
                return await httpClient.GetFromJsonAsync<InstructionModel>(requestUri);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                url = "v1/resources/agents/?email=" + options.EmailEmisor;
            }
            else
            {
                url = "v1/resources/agents/?email=" + username;
            }
            agente = (await httpClient
                   .GetFromJsonAsync<AgentModel>(url))
                   .Results.ToList()[0];
            tareas = agente.Participants.Select(async m =>
            {
                string requestUri = $"v1/resources/participants/?id={m.ParticipantID}";
                list.AddRange((await httpClient
                .GetFromJsonAsync<ParticipantModel>(requestUri)).Results);
                return list;
            }).ToList();
            await Task.WhenAll(tareas);
            return list.OrderBy(c => c.Id).ToList();
        }

        public async Task<string> ConvertDocument(string filepath)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(filepath);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Instrucción facturada, pero no existe el documento en CEN.");
            }
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }




        public async Task GetDocumentos(List<InstructionResult> instructions)
        {
            List<Task> tareas = new();
            tareas = instructions.Select(async m =>
                {
                    string requestUri = $"v1/resources/dtes/?reported_by_creditor=true&instruction={m.Id}";
                    List<DteResult> dteModel = (await httpClient.GetFromJsonAsync<DteModel>(requestUri)).Results.ToList();
                    if (dteModel != null && dteModel.Count > 0)
                    {
                        m.DteResult = dteModel;
                    }
                }).ToList();
            await Task.WhenAll(tareas);
        }
    }
}
