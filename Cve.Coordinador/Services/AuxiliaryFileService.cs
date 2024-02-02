using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

namespace Cve.Coordinador.Services
{
    internal class AuxiliaryFileService : IAuxiliaryFileService
    {
        private readonly IRepositoryBase repo;
        private readonly IAuthenticateService authenticate;
        private readonly JsonSerializerOptions options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public AuxiliaryFileService(IRepositoryBase repo, IAuthenticateService authenticate)
        {
            this.repo = repo;
            this.authenticate = authenticate;
        }

        public async Task<int> PutArchivo(string path, string namefile, CancellationToken ct)
        {
            using FileStream filestream = File.OpenRead(path);
            StreamContent reqcont = new(filestream);
            reqcont.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = namefile
            };
            try
            {
                string token = await authenticate.Authenticate(ct);
                HttpResponseMessage msg = await repo.PutStream(
                    Properties.Coordinador.UrlPutFiles,
                    reqcont,
                    token,
                    ct
                )!;
                await msg.EnsureSuccess();
                if (msg.IsSuccessStatusCode && msg.StatusCode == HttpStatusCode.Created)
                {
                    string body = await msg.Content.ReadAsStringAsync(ct);
                    dynamic obj = JsonNode.Parse(body)!.AsObject();
                    return (int)obj["invoice_file_id"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errror: PutArchivo {ex.Message}");
            }
            return 0;
        }

        public async Task<CreditorJobResult?> CrearJob(
            int fileId,
            int idCen,
            int idPm,
            CancellationToken ct
        )
        {
            var value = new
            {
                auxiliary_file = fileId,
                participant = idCen,
                payment_matrix = idPm
            };
            string contJson = JsonSerializer.Serialize(value);
            List<KeyValuePair<string, string>> values =
                new() { new KeyValuePair<string, string>("data", contJson) };
            FormUrlEncodedContent requestContent = new(values);
            string token = await authenticate.Authenticate(ct);
            try
            {
                HttpResponseMessage msg = await repo.PostForm(
                    Properties.Coordinador.UrlCreateJob,
                    requestContent,
                    ct,
                    token
                )!;
                if (msg.IsSuccessStatusCode && msg.StatusCode == HttpStatusCode.OK)
                {
                    string body = await msg.Content.ReadAsStringAsync(ct);
                    CreditorJob? res = await msg!.Content!.ReadFromJsonAsync<CreditorJob>(
                        options,
                        ct
                    )!;
                    return res!.Errors!.Count > 0 || res.Result!.Errors!.Count > 0
                        ? null!
                        : res.Result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errror: Crear Job {ex.Message}");
            }
            return null!;
        }

        public async Task<CreditorJobResult?> PublicarJob(
            CreditorJobResult jobResult,
            CancellationToken ct
        )
        {
            string token = await authenticate.Authenticate(ct);
            HttpResponseMessage response = await repo.PostForm(
                $"api/v1/operations/creditor-dte-load-jobs/{jobResult.Id}/publish/",
                null!,
                ct,
                token
            )!;

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                CreditorJob? res = await response.Content.ReadFromJsonAsync<CreditorJob>(
                    options,
                    ct
                )!;
                return res!.Errors!.Count > 0 || res.Result!.Errors!.Count > 0 ? null! : res.Result;
            }
            return null!;
        }
    }
}
