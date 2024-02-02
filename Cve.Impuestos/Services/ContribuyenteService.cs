using System.Net.Http.Json;
using System.Text;

using Cve.Impuestos.Connected_Services;
using Cve.Impuestos.Helpers;
using Cve.Impuestos.Infraestructure;
using Cve.Impuestos.Models;
using Cve.Impuestos.Services.Interfaces;

using Microsoft.Extensions.Configuration;

namespace Cve.Impuestos.Services
{
    internal class ContribuyenteService : WcfSemillaToken, IContribuyenteService
    {
        private readonly IRepositoryBaseWeb repo;
        private CaptchaModel? captchaModel = null!;

        public CaptchaModel? CaptchaModel
        {
            get
            {
                CaptchaModel? c = captchaModel;
                return c!;
            }
            set => captchaModel = value;
        }

        public ContribuyenteService(IRepositoryBaseWeb repo, IConfiguration config)
            : base(config)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Conecta al sitio con certificado digital y crea las cookies en HttpClient.
        /// </summary>
        /// <returns></returns>
        public async Task<IContribuyenteService> Conectar()
        {
            await repo.GenerarTokenSesion(Properties.Impuestos.UrlSolicitaFolios);
            return this;
        }

        /// <summary>
        /// Entra a la página antes de hacer la solicitud
        /// </summary>
        /// <param name="rut"></param>
        /// <param name="dv"></param>
        /// <returns></returns>
        public async Task<IContribuyenteService> ConsultaRut()
        {
            _ = await repo.SendAsync(Properties.Impuestos.UrlContribuyente, Token!)!;
            return this;
        }

        /// <summary>
        /// link:Consulta individual ctbyte.html
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> DescargaAntecedentes(
            string rut,
            string dv,
            CancellationToken token
        )
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>("RUT_EMP", rut),
                    new KeyValuePair<string, string>("DV_EMP", dv),
                    new KeyValuePair<string, string>("ACEPTAR", "Consultar")
                };
            using HttpResponseMessage? msg = await repo.PostFormWeb(
                values,
                token,
                Properties.Impuestos.UrlContribuyenteByRut
            )!;
            await EnsureSuccessStatus(msg);
            return await HtmlParse.GetValuesFromTabla(
                "body > center:nth-child(2) > table:nth-child(3) > tbody",
                msg,
                new CancellationToken()
            );
        }

        /// <summary>
        /// Archivo CVS de Contribuyentes Autorizados (email de intercambio DTE)
        /// </summary>
        /// <returns></returns>
        public async Task<string> DescargaFile()
        {
            using HttpResponseMessage? msg = await repo.SendAsync(
                Properties.Impuestos.UrlContribuyenteFile,
                Token!
            )!;
            await EnsureSuccessStatus(msg);
            string? filename = msg.Content.Headers!.ContentDisposition!.FileName;
            using (Stream streamToReadFrom = await msg.Content.ReadAsStreamAsync())
            {
                using Stream streamToWriteTo = File.Open(
                    $"{Path.GetTempPath()}{filename}",
                    FileMode.Create
                );
                await streamToReadFrom.CopyToAsync(streamToWriteTo);
            }
            return filename!;
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

        /// <summary>
        /// Obtiene número Captcha
        /// </summary>
        /// <returns></returns>
        public async Task<IContribuyenteService> GetCaptcha()
        {
            if (captchaModel == null)
            {
                using HttpResponseMessage? msg = await repo.PostApiJson(
                    "oper=0",
                    Properties.Impuestos.UrlGetCaptcha,
                    Token!,
                    new CancellationToken()
                )!;
                if (msg != null)
                {
                    captchaModel = await msg.Content!.ReadFromJsonAsync<CaptchaModel>();
                }
            }
            return this;
        }

        /// <summary>
        /// Obtiene ACTECO de Situación Tributaria
        /// link:Consulta tributaria 3ro.html
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> SituacionTributariaTerceros(
            string ruttercero,
            string dvtercero,
            CancellationToken token
        )
        {
            _ = new Dictionary<string, string>();
            string? captcha = Encoding.UTF8
                .GetString(Convert.FromBase64String(captchaModel!.TxtCaptcha!))
                .Substring(36, 4);
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>("RUT", ruttercero),
                    new KeyValuePair<string, string>("DV", dvtercero),
                    new KeyValuePair<string, string>("txt_captcha", captchaModel!.TxtCaptcha!),
                    new KeyValuePair<string, string>("txt_code", captcha),
                    new KeyValuePair<string, string>("PRG", "STC"),
                    new KeyValuePair<string, string>("OPC", "NOR"),
                    new KeyValuePair<string, string>("ACEPTAR", "Consultar situación tributaria")
                };
            using HttpResponseMessage? msg = await repo.PostFormWeb(
                values,
                token,
                Properties.Impuestos.UrlSituacionTributariaTerceros
            )!;
            await EnsureSuccessStatus(msg);
            return await HtmlParse.GetValuesFromTabla(
                "#contenedor > table:nth-child(27) > tbody",
                msg,
                token
            );
        }
    }
}
