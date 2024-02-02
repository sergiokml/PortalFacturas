using System.Text.Json;
using System.Text.Json.Serialization;

using Cve.Impuestos.Connected_Services;
using Cve.Impuestos.Infraestructure;
using Cve.Impuestos.Models;
using Cve.Impuestos.Services.Interfaces;

using Microsoft.Extensions.Configuration;

namespace Cve.Impuestos.Services
{
    public class MisDteService : WcfSemillaToken, IMisDteService
    {
        public const string exportacompra =
            "cl.sii.sdi.lob.diii.consdcv.data.api.interfaces.FacadeService/getDetalleCompraExport";
        public const string exportaventa =
            "cl.sii.sdi.lob.diii.consdcv.data.api.interfaces.FacadeService/getDetalleVentaExport";
        private readonly IRepositoryBaseWeb repo;
        private readonly JsonSerializerOptions options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        public static string? UrlCsvEmitidos => Properties.Impuestos.UrlExportCvsEmitidos;
        public static string? UrlCsvRecibidos => Properties.Impuestos.UrlExportCvsRecibidos;
        public static string? UrlDteRecibidos => Properties.Impuestos.UrlMisDteRecibidos;
        public static string? UrlDteEmitidos => Properties.Impuestos.UrlMisDteEmitidos;

        public MisDteService(IRepositoryBaseWeb repo, IConfiguration config)
            : base(config)
        {
            this.repo = repo;
        }

        public async Task<DescargaCsv?> DescargarCsv(Data data, string ns, string url)
        {
            await GetToken();
            MetaData meta =
                new()
                {
                    Namespace = ns,
                    TransactionId = "0",
                    ConversationId = Token
                };
            DetalleReq? dte = new(meta, data);
            string? json = JsonSerializer.Serialize(dte);
            HttpResponseMessage? msg = await repo.PostApiJson(
                json,
                url,
                Token!,
                new CancellationToken()
            )!;
            await EnsureSuccessStatus(msg);
            return JsonSerializer.Deserialize<DescargaCsv>(
                await msg.Content.ReadAsStreamAsync(),
                options
            );
        }

        public override async Task EnsureSuccessStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await GetToken();
                }
                else
                {
                    throw new Exception(
                        $"{response.StatusCode}:{response.RequestMessage!.RequestUri}"
                    );
                }
            }
        }

        public async Task<ResDetalle?> GetDetalle(
            MetaData meta,
            Data data,
            string url,
            CancellationToken canceltoken
        )
        {
            meta.ConversationId = Token;
            ReqDetalle? dte = new(meta, data);
            string? json = JsonSerializer.Serialize(dte);
            HttpResponseMessage? msg = await repo.PostApiJson(json, url, Token!, canceltoken)!;
            await EnsureSuccessStatus(msg);
            return JsonSerializer.Deserialize<ResDetalle>(
                await msg.Content.ReadAsStreamAsync(),
                options
            );
        }

        public async Task<ResResumen?> GetResumen(MetaData meta, Data data)
        {
            // LLAMAR TOKEN SOLO EN RESUMEN, NO ES NECESARIO EN DETALLE EMITIDO/RECIBIDO
            await GetToken();
            meta.ConversationId = Token;
            ReqDetalle? dte = new(meta, data);
            HttpResponseMessage? msg = await repo!.PostApiJson(
                JsonSerializer.Serialize(dte)!,
                Properties.Impuestos.UrlMisDteResumen!,
                Token!,
                new CancellationToken()
            )!;
            await EnsureSuccessStatus(msg);
            return JsonSerializer.Deserialize<ResResumen>(
                await msg.Content.ReadAsStreamAsync(),
                options
            );
        }
    }
}
