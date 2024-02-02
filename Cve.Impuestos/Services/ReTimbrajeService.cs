using Cve.Impuestos.Helpers;
using Cve.Impuestos.Infraestructure;
using Cve.Impuestos.Services.Interfaces;

namespace Cve.Impuestos.Services
{
    internal class ReTimbrajeService : IReTimbrajeService
    {
        private readonly IRepositoryBaseWeb repo;

        private Dictionary<string, string> InputsText { get; set; }

        public ReTimbrajeService(IRepositoryBaseWeb repo)
        {
            this.repo = repo;
            InputsText = new Dictionary<string, string>();
        }

        public async Task<IReTimbrajeService> Conectar()
        {
            await repo.GenerarTokenSesion(Properties.Impuestos.UrlReObtencionConecta!);
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<string> DescargaFolio(CancellationToken token)
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>(
                        "RUT_EMP",
                        InputsText.GetValueOrDefault("RUT_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "DV_EMP",
                        InputsText.GetValueOrDefault("DV_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "COD_DOCTO",
                        InputsText.GetValueOrDefault("COD_DOCTO")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_INI",
                        InputsText.GetValueOrDefault("FOLIO_INI")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_FIN",
                        InputsText.GetValueOrDefault("FOLIO_FIN")!
                    ),
                    new KeyValuePair<string, string>(
                        "FECHA",
                        $"{InputsText.GetValueOrDefault("FECHA")!}"
                    ),
                    new KeyValuePair<string, string>("ACEPTAR", "AQUI")
                };

            string? filename;
            HttpResponseMessage? msg;
            try
            {
                msg = await repo.PostFormWeb(
                    values,
                    token,
                    Properties.Impuestos.UrlReObtencionGeneraArchivo
                )!;
                filename = msg.Content.Headers!.ContentDisposition!.FileName;
            }
            catch (Exception)
            {
                return null!;
            }
            using (Stream streamToReadFrom = await msg.Content.ReadAsStreamAsync())
            {
                using Stream streamToWriteTo = File.Open(
                    $"{Directory.GetCurrentDirectory()}\\{filename}",
                    FileMode.Create
                );
                await streamToReadFrom.CopyToAsync(streamToWriteTo);
            }
            return $"{filename!}-{InputsText["FECHA"]}";
        }

        /// <summary>
        ///
        /// link:Descargar timbraje.html
        /// </summary>
        /// <returns></returns>
        public async Task<IReTimbrajeService> GeneraFolios(CancellationToken token)
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>(
                        "RUT_EMP",
                        InputsText.GetValueOrDefault("RUT_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "DV_EMP",
                        InputsText.GetValueOrDefault("DV_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "COD_DOCTO",
                        InputsText.GetValueOrDefault("COD_DOCTO")!
                    ),
                    new KeyValuePair<string, string>(
                        "NOMBRE_DOCTO",
                        InputsText.GetValueOrDefault("NOMBRE_DOCTO")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_INI",
                        InputsText.GetValueOrDefault("FOLIO_INI")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_FIN",
                        InputsText.GetValueOrDefault("FOLIO_FIN")!
                    ),
                    new KeyValuePair<string, string>(
                        "CANT_DOCTOS",
                        InputsText.GetValueOrDefault("CANT_DOCTOS")!
                    ),
                    new KeyValuePair<string, string>("DIA", InputsText.GetValueOrDefault("DIA")!),
                    new KeyValuePair<string, string>("MES", InputsText.GetValueOrDefault("MES")!),
                    new KeyValuePair<string, string>("ANO", InputsText.GetValueOrDefault("ANO")!),
                    new KeyValuePair<string, string>("ACEPTAR", "Solicitar Folios")
                };
            HttpResponseMessage? msg = await repo.PostFormWeb(
                values,
                token,
                Properties.Impuestos.UrlReObtencionGeneraFolio
            )!;
            InputsText = await HtmlParse.GetValuesFromTag(
                "input[type='text'],input[type='hidden']",
                msg,
                token
            );
            return this;
        }

        /// <summary>
        ///
        /// link:Confirma reobtencion.html
        /// </summary>
        /// <returns></returns>
        public async Task<IReTimbrajeService> SeleccionaFolios(CancellationToken token)
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>(
                        "PAGINA",
                        InputsText.GetValueOrDefault("PAGINA")!
                    ),
                    new KeyValuePair<string, string>(
                        "CANT_DOCTOS",
                        InputsText.GetValueOrDefault("CANT_DOCTOS")!
                    ),
                    new KeyValuePair<string, string>(
                        "RUT_EMP",
                        InputsText.GetValueOrDefault("RUT_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "DV_EMP",
                        InputsText.GetValueOrDefault("DV_EMP")!
                    ),
                    new KeyValuePair<string, string>("DIA", InputsText.GetValueOrDefault("DIA")!),
                    new KeyValuePair<string, string>("ANO", InputsText.GetValueOrDefault("ANO")!),
                    new KeyValuePair<string, string>("MES", InputsText.GetValueOrDefault("MES")!),
                    new KeyValuePair<string, string>(
                        "COD_DOCTO",
                        InputsText.GetValueOrDefault("COD_DOCTO")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_INI",
                        InputsText.GetValueOrDefault("FOLIO_INI")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_FIN",
                        InputsText.GetValueOrDefault("FOLIO_FIN")!
                    )
                };
            HttpResponseMessage? msg = await repo.PostFormWeb(
                values,
                token,
                Properties.Impuestos.UrlReObtencionConfirma
            )!;
            InputsText = await HtmlParse.GetValuesFromTag(
                "input[type='text'],input[type='hidden']",
                msg,
                new CancellationToken()
            );
            return this;
        }

        /// <summary>
        ///
        /// link:Tabla reobtenciones.html
        /// </summary>
        /// <param name="rut"></param>
        /// <param name="dv"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public async Task<IReTimbrajeService> SolicitaFolios(
            string rut,
            string dv,
            string tipo,
            CancellationToken token
        )
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>("RUT_EMP", rut),
                    new KeyValuePair<string, string>("DV_EMP", dv),
                    new KeyValuePair<string, string>("COD_DOCTO", tipo),
                    new KeyValuePair<string, string>("PAGINA", "1"),
                    new KeyValuePair<string, string>("ACEPTAR", "Consultar")
                };

            HttpResponseMessage? msg = await repo.PostFormWeb(
                values,
                token,
                Properties.Impuestos.UrlReObtencionFolios
            )!;
            InputsText = await HtmlParse.GetValuesFromTag(
                "input[type='text'],input[type='hidden']",
                msg,
                token
            );
            return this;
        }
    }
}
