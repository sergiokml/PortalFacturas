
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using PortalFacturas.Interfaces;
using PortalFacturas.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace PortalFacturas.Services
{
    public class ApiCenService : IApiCenService
    {
        private readonly HttpClient httpClient;
        //private readonly JsonSerializerOptions jsonSerializerOptions;
        private readonly OptionsModel options;

        public IConfiguration Configuration { get; }

        public ApiCenService(HttpClient httpClient, IConfiguration configuration, OptionsModel options)
        {
            Configuration = configuration;
            this.httpClient = httpClient;
            this.options = options;

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
            return ((dynamic)(await (await httpClient.PostAsJsonAsync("token-auth/", value)).Content.ReadFromJsonAsync<TokenAuth>()))?.Token;
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
                url = "v1/resources/agents/?email=" + options.UserName;
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
                IEnumerable<ParticipantResult> ag = (await httpClient
                .GetFromJsonAsync<ParticipantModel>(requestUri)).Results;
                list.AddRange(ag);
                return list;
            }).ToList();
            await Task.WhenAll(tareas);
            return list;
        }

        public async Task<string> GetXmlFile(string filepath)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(filepath);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Instrucción facturada, pero no existe el documento en CEN.");
            }
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<byte[]> GetPdfFile(string filepath)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(filepath);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Instrucción facturada, pero no existe el documento en CEN.");
            }
            return await httpResponseMessage.Content.ReadAsByteArrayAsync();
        }

        public async Task<ResponseModel> UploadToFunctionAzure(string res)
        {
            UploadModel uploadModel = new()
            {
                InputXml = res
            };
            uploadModel.Mapper.Directory = "";
            uploadModel.Mapper.Name = "Custodium.xslt";
            HttpContent content2 = new StringContent(JsonSerializer.Serialize(uploadModel), Encoding.UTF8, "application/json");
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(options.UrlFunction),
                Content = content2
            };
            return await (await httpClient.SendAsync(request)).Content.ReadFromJsonAsync<ResponseModel>();
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
                        m.DteResult = dteModel[0];
                    }
                }).ToList();
            await Task.WhenAll(tareas);
        }

        public async Task<byte[]> ConvertToPdf(string content)
        {
            var postData = new
            {
                html = content,
                json = "true",
                pdf_page = "Letter"

            };
            string contentt = JsonSerializer.Serialize(postData);
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://restpack.io/api/html2pdf/v7/convert"),
                Content = new StringContent(contentt, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("x-access-token", "Tc2ENx8UpsBwERyhmCy42t0hfVCxubU4GCpoXUDQ98q0DfHP");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"No se pudo convertir el documento.(restack.io)");
            }
            ResponsePdfRestPackModel pdf = await response.Content.ReadFromJsonAsync<ResponsePdfRestPackModel>();
            return await GetPdfFile(pdf.Image);
        }
    }
}
